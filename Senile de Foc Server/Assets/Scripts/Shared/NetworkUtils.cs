using UnityEngine;
using System.Collections;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.IO;
using System.Text;

using Object = System.Object;

public class NetworkUtils : MonoBehaviour 
{
	public static byte[] ObjectToByteArray (Object obj)
	{
		using (MemoryStream ms = new MemoryStream ())
		{
			BinaryFormatter bf = new BinaryFormatter ();
			bf.Serialize (ms, obj);
			return ms.ToArray ();
		}
	}
	
	public static Object ByteArrayToObject (byte[] byteArray)
	{
		using (var ms = new MemoryStream ())
		{
			BinaryFormatter bf = new BinaryFormatter ();
			ms.Write (byteArray, 0, byteArray.Length);
			ms.Seek (0, SeekOrigin.Begin);
			Object obj = bf.Deserialize (ms);
			return obj;
		}
	}
}
