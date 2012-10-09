using UnityEngine;
using ExtensionMethods;

/** Utility behaviour for destroying network objects.
 *
 * Destroys the gameObject (using Network.Destroy) and makes server unbuffer the RPCs for the object,
 * so that later connections won't get the buffered Network.Instantiate (using Network.RemoveRPCs on the server).
 *
 * Use like:
 *     GetComponent<NetworkDestroyer> ().DestroyAndUnbuffer ();
 */
[RequireComponent (typeof (NetworkView))]
public class NetworkDestroyer : MonoBehaviour
{
	/** Tells the server to unbuffer any RPCs for this object, and then calls Network.Destroy. */
	public void DestroyAndUnbuffer ()
	{
		UnbufferRPCs ();
		Network.Destroy (gameObject);
	}

	[RPC]
	public void UnbufferRPCs () {
		if (!networkView.Server ("UnbufferRPCs")) {
			return;
		}

		Network.RemoveRPCs (networkView.viewID);
	}
}
