using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Countdown : MonoBehaviour 
{
	Text textHandle;
	public string toAppend;

	float _val;
	float val
	{
		get { return _val; }
		set
		{
			_val = value;
			textHandle.text = Utils.FloatToTime (_val) + toAppend;
		}
	}

	void Awake ()
	{
		textHandle = GetComponent <Text> ();
		textHandle.text = "";
	}

	Action onCompletion = null;
	// Not Start because it is taken by UnityEngine / MonoBehaviour
	public void StartIt (float time, Action onCompletion = null)
	{
		if (time <= 0)
			Debug.LogErrorFormat ("Countdown started with time {0}", time);

		val = time;
		this.onCompletion = onCompletion;
	}

	void Update ()
	{
		if (textHandle.enabled) {
			val -= Time.deltaTime;

			if (val <= 0)
				Finished ();
		}
	}

	void Finished ()
	{
		textHandle.text = "";

		if (onCompletion != null)
			onCompletion ();
	}

}
