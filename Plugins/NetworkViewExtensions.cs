using UnityEngine;

namespace ExtensionMethods
{
	public static class NetworkViewExtensions
	{
		/** Same as Network.RemoveRPCs, but ensure it is executed on the server. */
		[RPC]
		public static void RemoveRPCsOnServer (this NetworkView networkView)
		{
			if (Network.isServer) {
				Network.RemoveRPCs (networkView.viewID);
			} else {
				networkView.RPC ("RemoveRPCsOnServer", RPCMode.Server);
			}
		}
	}
}
