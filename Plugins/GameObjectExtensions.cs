using UnityEngine;

namespace ExtensionMethods
{
	public static class GameObjectExtensions
	{
		public static int RecursiveLayerMask(this GameObject go)
		{
			LayerMask mask = 1 << go.layer;
			foreach (Transform t in go.transform)
			{
				mask |= t.gameObject.RecursiveLayerMask ();
			}
			return mask;
		}

		public static Bounds GetEncapsulatingBounds (this GameObject go)
		{
			Bounds bounds = new Bounds (Vector3.zero, Vector3.zero);
			if (go.renderer != null)
				bounds = go.renderer.bounds;

			foreach (Renderer r in go.GetComponentsInChildren<Renderer> ())
			{
				bounds.Encapsulate (r.bounds);
			}

			return bounds;
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

