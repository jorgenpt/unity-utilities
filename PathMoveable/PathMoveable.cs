// Copyright (c) 2012, Jørgen P. Tjernø <jorgenpt@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

// Uncomment the following line to see a lot of details in the Debug window when we get a new path.
//#define DEBUG_PATH

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
[RequireComponent (typeof (Navigator))]
public class PathMoveable : MonoBehaviour
{
	// Units per second.
	public float speed = 10f;

	// Whether or not we ignore the y component (height) of incoming 'move' requests.
	public bool shouldIgnoreHeightOfDestination = true;

	// If we're less than this many seconds from arriving at a destination, we say it's good enough.
	public float remainingDistanceTolerance = 0.05f;

	// State about current destination, currentPath == null means we're not moving anywhere.
	Path currentPath;
	Waypoint currentWaypoint;
	Vector3 currentDestination;

	void Update ()
	{
		if (currentPath != null)
		{
			Vector3 toDestination = (currentDestination - transform.position);
			toDestination.y = 0f;

			if (Vector3.Distance (transform.position, currentPath.EndPosition) < remainingDistanceTolerance * speed)
				StopMoving ();
			else if (toDestination.magnitude < remainingDistanceTolerance * speed)
				ArrivedAtDestination ();
			else
				((CharacterController)collider).SimpleMove (toDestination.normalized * speed);
		}
	}

	void ArrivedAtDestination ()
	{
		if (currentWaypoint == null)
		{
			// This means there are no more waypoints, so we're there. Yay!
			StopMoving ();
		}
		else
		{
			// This removes currentWaypoint from currentPaht.Segments.
			currentPath.ArrivedAt (currentWaypoint);

			if (currentPath.Segments.Count > 0)
			{
				// If there are any Segments of this Path left, then navigate to the end of the first remaining segment.
				currentWaypoint = currentPath.Segments[0].To;
				currentDestination = currentWaypoint.Position;
			}
			else
			{
				// Otherwise, we're through navigating the segments and can go directly to the end position.
				currentWaypoint = null;
				currentDestination = currentPath.EndPosition;
			}
		}
	}

	// Move to a given position.
	public void StartMovingTo (Vector3 position)
	{
		if (shouldIgnoreHeightOfDestination)
			position.y = transform.position.y;

		GetComponent<Navigator> ().targetPosition = position;
	}

	// Stop moving completely.
	public void StopMoving ()
	{
		GetComponent<Navigator> ().targetPosition = transform.position;
		currentPath = null;
	}

	void OnNewPath (Path path)
	{
		if (path == null)
		{
			Debug.Log ("OnNewPath(null), stopping movement.");
			StopMoving ();
			return;
		}

		currentPath = path;
		// For some reason, it sometimes gives us direct paths that go via an arbitrary waypoint.
		if (path.StartNode == null || GetComponent<Navigator> ().DirectPath (path.EndPosition))
		{
			currentWaypoint = null;
			currentDestination = path.EndPosition;
		}
		else
		{
			currentWaypoint = path.StartNode;
			currentDestination = currentWaypoint.Position;
		}

#if DEBUG_PATH
		Debug.Log ("Start position: " + path.StartPosition);
		Debug.Log ("Start node: " + path.StartNode + (path.StartNode != null ? ", position:" + path.StartNode.Position : ""));

		int i = 1;
		foreach (Connection conn in path.Segments)
			Debug.Log (" Segment #" + i++ + ": " + conn.From + " -> " + conn.To);

		Debug.Log ("End position: " + path.EndPosition);
		Debug.Log ("End node: " + path.EndNode + (path.EndNode != null ? ", position:" + path.EndNode.Position : ""));
#endif
	}

	void OnTargetUnreachable ()
	{
		currentPath = null;
		Debug.Log ("Could not pathfind to target position");
	}

	void OnPathInvalidated (Path path)
	{
		Debug.Log ("The path from " + path.StartNode + " to " + path.EndNode + " is no longer valid.");
		if (path == currentPath)
		{
			// Try to recalculate the path.
			StopMoving ();
			GetComponent<Navigator> ().ReSeek ();
		}
	}

	void OnDrawGizmos ()
	{
		if (currentPath != null)
			currentPath.OnDrawGizmos ();
	}
}

// vim: set noexpandtab sw=8 sts=8 ts=8:
