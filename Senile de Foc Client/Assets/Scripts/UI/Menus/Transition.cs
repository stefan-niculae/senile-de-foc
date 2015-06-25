using UnityEngine;
using System.Collections;
using System;

public class Transition : MonoBehaviour 
{
	public enum Properties { scale, position, rotation };
	[HideInInspector] public Properties property;

	Vector3 start;
	Vector3 end;
	Vector3 delta;

	float startTime;
	float duration;
	float errorMargin;

	float defaultDuration;
	Action onFinish;


	public void Initialize (Properties property, Vector3 initialValue, float defaultDuration = float.NaN)
	{
		this.property = property;
		end = initialValue;
		ChangeTo (initialValue);

		this.defaultDuration = defaultDuration;
	}

	Vector3 Current ()
	{
		if (property == Properties.scale)
			return transform.localScale;
		if (property == Properties.position)
			return transform.localPosition;
		if (property == Properties.rotation)
			return transform.localRotation.eulerAngles;

		return Vector3.zero; // will never reach this
	}

	void ChangeTo (Vector3 value)
	{
		if (property == Properties.scale)
			transform.localScale = value;
		if (property == Properties.position)
			transform.localPosition = value;
		if (property == Properties.rotation)
			transform.localRotation = Quaternion.Euler (value);
	}
	
	public void TransitionTo (Vector3 destination, float duration = float.NaN, Action callback = null)
	{
		if (float.IsNaN (duration))
			duration = defaultDuration;
		if (callback == null)
			callback = Constants.DO_NOTHING;

		start = Current ();
		end = destination;
		delta = end - start;

		startTime = Time.time;
		this.duration = duration;
		errorMargin = Vector3.Distance (start, end) * .125f; // due to the imprecissions of working with time

		onFinish = callback;
	}

	void Update ()
	{
		if (!ReachedDestination ()) {
			float coeff = (Time.time - startTime) / duration;
			ChangeTo (start + coeff * delta);

			if (ReachedDestination ()) {
				ChangeTo (end);
				onFinish ();
			}
		} 
	}

	bool ReachedDestination ()
	{
		return Vector3.Distance (Current (), end) <= errorMargin;
	}

}
