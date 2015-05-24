using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkStatus : Singleton <NetworkStatus> 
{
	public enum MessageType { working, success, failure };

	static Text text;

	public Color working, success, failure;

	void Awake ()
	{
		text = GetComponent <Text> ();
	}

	public static void Show (string message, MessageType type)
	{
		text.text = message;
		switch (type)
		{
			case MessageType.working:
				text.color = Instance.working;
				break;

			case MessageType.success:
				text.color = Instance.success;
				break;

			case MessageType.failure:
				text.color = Instance.failure;
				break;
		}
	}
}
