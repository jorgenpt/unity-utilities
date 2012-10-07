using UnityEngine;
using System.Collections.Generic;

namespace ExtensionMethods
{
	public static class LayerMaskExtensions
	{
		/** Return what layers are included in this LayerMask. */
		public static List<int> GetLayers (this LayerMask layerMask)
		{
			var layers = new List<int> ();
			for (int mask = layerMask.value, layer = 0; mask != 0; mask = mask >> 1, layer++)
			{
				if ((mask & 1) != 0)
					layers.Add (layer);
			}

			return layers;
		}
	}
}

