using UnityEngine;
using System;
using System.Reflection;

namespace ExtensionMethods
{
	public static class ComponentExtensions
	{
		/** Like SendMessage, except it allows you to pass multiple arguments. */
		public static void SendMessagePlus (this Component component, string name, SendMessageOptions options, params object[] args)
		{
			Type[] types = Type.GetTypeArray (args);

			var behaviours = component.GetComponents<MonoBehaviour> ();
			foreach (var behaviour in behaviours) {
				MethodInfo method = null;
				try {
					method = behaviour.GetType ().GetMethod (name, types);
				} catch (AmbiguousMatchException e) {
					Debug.LogError ("More than one method on " + behaviour + " matches: " + e);
					return;
				} catch (Exception e) {
					Debug.LogError (e);
				}

				if (method != null) {
					method.Invoke (behaviour, args);
					return;
				}
			}

			if (options == SendMessageOptions.RequireReceiver) {
				Debug.LogError ("LPC " + name + " has no receiver!");
			}
		}

		/** Local Procedure Call. Combines the benefits of NetworkView.RPC (variable arguments) with the functionality of SendMessage. */
		public static void LPC (this Component component, string name, params object[] args)
		{
			component.LPC (name, SendMessageOptions.RequireReceiver, args);
		}
	}
}

