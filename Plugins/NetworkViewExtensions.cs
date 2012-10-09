using UnityEngine;

namespace ExtensionMethods
{
	public static class NetworkViewExtensions
	{
		/** Used in a method called from the owner of a NetworkView, to ensure that everyone else also runs this code. Returns whether or not we're the owner or "others".
		 *
		 * This is used like:
		 *
		 * [RPC]
		 * void SetPosition (Vector3 position) {
		 *     networkView.Others ("SetPosition", position);
		 *     mPosition = position;
		 * }
		 *
		 * If you call this method on the owner, it'll automatically send it as an RPC to everyone else, and ensure they don't also
		 * send the RPC.
		 *
		 * Idea from http://answers.unity3d.com/users/21269/whydoidoit.html on http://answers.unity3d.com/questions/329453/is-it-possible-to-rpc-a-c-extension-method.html
		 */
		public static bool Others (this NetworkView networkView, string name, params object[] parameters)
		{
			if (networkView.isMine) {
				networkView.RPC (name, RPCMode.Others, parameters);
				return false;
			}

			return true;
		}

		/** Used to ensure the method is executed on the server. Returns true if we're the server, otherwise sends a server RPC and returns false.
		 *
		 * This is used like:
		 *
		 * [RPC]
		 * void ApplyDamage (float damage) {
		 *     if (!networkView.Server ("ApplyDamage", damage)) {
		 *         return;
		 *     }
		 *     health -= damage;
		 * }
		 *
		 * If you call this method on the client, it'll automatically send it as an RPC to the server, and make no change on the client.
		 *
		 * Idea from http://answers.unity3d.com/users/21269/whydoidoit.html on http://answers.unity3d.com/questions/329453/is-it-possible-to-rpc-a-c-extension-method.html
		 */
		public static bool Server (this NetworkView networkView, string name, params object[] parameters)
		{
			if (Network.isServer) {
				return true;
			}

			networkView.RPC (name, RPCMode.Server, parameters);
			return false;
		}
	}
}
