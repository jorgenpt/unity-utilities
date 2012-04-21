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
	}
}

