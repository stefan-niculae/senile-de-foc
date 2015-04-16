using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Countdown : MonoBehaviour 
{
	public GameObject overlay = null;

	Text textHandle;

	float _val;
	float val
	{
		get { return _val; }
		set
		{
			_val = value;
			textHandle.text = val.ToString ("F2");
		}
	}

	void Awake ()
	{
		textHandle = GetComponent <Text> ();
		textHandle.enabled = false;

		if (overlay != null)
			overlay.SetActive (false);
	}

	Action onCompletion = null;
	// Not Start because it is taken by UnityEngine / MonoBehaviour
	public void StartIt (float time, Action onCompletion = null)
	{
		if (time <= 0)
			Debug.LogErrorFormat ("Countdown started with time {0}", time);

		val = time;

		if (overlay != null)
			overlay.SetActive (true);
		textHandle.enabled = true;

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
		textHandle.enabled = false;
		overlay.SetActive (false);

		if (onCompletion != null)
			onCompletion ();
	}

}
