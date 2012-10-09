using UnityEngine;
using System.Linq;

namespace ExtensionMethods
{
	public static class GameObjectExtensions
	{
		/** Returns a LayerMask that covers this object and any children. */
		public static int RecursiveLayerMask (this GameObject go)
		{
			LayerMask mask = 1 << go.layer;
			foreach (Transform t in go.transform) {
				mask |= t.gameObject.RecursiveLayerMask ();
			}
			return mask;
		}

		/** Returns Bounds that encapsulate this GameObject's renderer and
		 * that of any children that match `mask`.
		 */
		public static Bounds GetEncapsulatingBounds (this GameObject go, LayerMask mask)
		{
			var renderers =
				from r in go.GetComponentsInChildren<Renderer> ()
					where (mask & r.gameObject.layer) != 0
					select r;

			if (renderers.Count () == 0)
				return new Bounds (Vector3.zero, Vector3.zero);

			Bounds bounds = renderers.First ().bounds;
			renderers = renderers.Skip (1);
			foreach (var renderer in renderers)
				bounds.Encapsulate (renderer.bounds);

			return bounds;
		}

		/** Same as GetEncapsulatingBounds, but include *all* children. */
		public static Bounds GetEncapsulatingBounds (this GameObject go)
		{
			return go.GetEncapsulatingBounds (~0);
		}

		/** Check if this GameObject's renderer or any child renderer that matches `mask`
		 * is currently visible on screen.
		 */
		public static bool IsAnyPartVisible (this GameObject go, LayerMask mask)
		{
			var renderers =
				from r in go.GetComponentsInChildren<Renderer> ()
					where (mask & r.gameObject.layer) != 0 && r.isVisible
					select r;

			return renderers.Count () > 0;
		}

		/** Same as IsAnyPartVisible, but include *all* children. */
		public static bool IsAnyPartVisible (this GameObject go)
		{
			return go.IsAnyPartVisible (~0);
		}

		public static Bounds GetEncapsulatingLocalBounds (this GameObject go)
		{
			Bounds bounds = new Bounds (Vector3.zero, Vector3.zero);
			MeshFilter mf = go.GetComponent<MeshFilter> ();
			if (mf != null && mf.sharedMesh != null)
				bounds = mf.sharedMesh.bounds;
			else if (go.renderer is SkinnedMeshRenderer)
				bounds = ((SkinnedMeshRenderer)go.renderer).localBounds;

			foreach (MeshFilter m in go.GetComponentsInChildren<MeshFilter> ())
				bounds.Encapsulate (m.sharedMesh.bounds);
			foreach (SkinnedMeshRenderer m in go.GetComponentsInChildren<SkinnedMeshRenderer> ())
				bounds.Encapsulate (m.localBounds);

			return bounds;
		}
	}
}

