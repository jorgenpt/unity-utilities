using UnityEngine;

namespace ExtensionMethods
{
	public static class Vector3Extensions
	{
		/** Returns a copy of this vector with the given x component. */
		public static Vector3 WithX (this Vector3 vec, float x)
		{
			return new Vector3(x, vec.y, vec.z);
		}

		/** Returns a copy of this vector with the given y component. */
		public static Vector3 WithY (this Vector3 vec, float y)
		{
			return new Vector3(vec.x, y, vec.z);
		}

		/** Returns a copy of this vector with the given z component. */
		public static Vector3 WithZ (this Vector3 vec, float z)
		{
			return new Vector3(vec.x, vec.y, z);
		}
	}
}
