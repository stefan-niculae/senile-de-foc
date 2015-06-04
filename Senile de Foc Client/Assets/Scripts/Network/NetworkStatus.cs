using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkStatus : Singleton <NetworkStatus> 
{
	const float TIMEOUT = 10; // in seconds
	float fadeStartTime;
	const float FADE_DURATION = 2.5f; // in seconds
	bool fading;

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

		Instance.SetTransparency (1);
		Instance.fadeStartTime = Time.time + TIMEOUT;
	}

	void Update ()
	{
		if (fadeStartTime <= Time.time && !fading) 
			fading = true;

		if (fading) {
			if (Time.time >= fadeStartTime + FADE_DURATION)
				fading = false;
			else 
				SetTransparency (1 - (Time.time - fadeStartTime) / FADE_DURATION);
		}

	}

	void SetTransparency (float amount)
	{
		var col = text.color;
		col.a = amount;
		text.color = col;
	}

}
