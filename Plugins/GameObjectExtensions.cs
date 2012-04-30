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
			Renderer[] renderers = go.GetComponentsInChildren<Renderer> ();
	
			if (renderers.Length == 0)
				return new Bounds (Vector3.zero, Vector3.zero);
			
			Bounds bounds = renderers[0].bounds;
			for (int i = 1; i < renderers.Length; ++i)
				bounds.Encapsulate (renderers[i].bounds);

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

