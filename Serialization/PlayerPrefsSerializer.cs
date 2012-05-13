using UnityEngine;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerPrefsSerializer<T> where T : ISerializable
{
	public static void Save (string name, T obj)
	{
		// Serialize state to memory.
		Stream stream = new MemoryStream ();
		BinaryFormatter bf = new BinaryFormatter ();
		bf.Serialize (stream, obj);

		// Read stream back to a byte array.
		stream.Seek (0, SeekOrigin.Begin);
		byte[] stateData = new byte[stream.Length];
		stream.Read (stateData, 0, (int)stream.Length);
		stream.Close ();

		// Convert to base64 and store in playerprefs.
		PlayerPrefs.SetString (name, System.Convert.ToBase64String (stateData));
	}

	public static T Load (string name)
	{
		T loaded = default (T);
		if (PlayerPrefs.HasKey (name))
		{
			byte[] stateData = System.Convert.FromBase64String (PlayerPrefs.GetString (name));
			Stream stream = new MemoryStream (stateData);
			BinaryFormatter bf = new BinaryFormatter ();

			try {
				loaded = (T)bf.Deserialize (stream);
			} catch {
				Debug.LogWarning ("Unable to deserialize data for key, deleting: " + name);
				PlayerPrefs.DeleteKey (name);
			}

			stream.Close ();
		}

		return loaded;
	}
}
