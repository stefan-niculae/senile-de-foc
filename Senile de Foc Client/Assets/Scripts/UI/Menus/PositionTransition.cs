using UnityEngine;
using System.Collections;
using System;

public class PositionTransition : MonoBehaviour 
{
	const float DURATION = .25f;
	Vector3 expandedPosition;
	Vector3 collapsedPosition;
	public Vector3 deltaPosition;

	float chosenDuration;
	Action onFinish;

	
	Vector3 endPosition;
	void Start ()
	{
		collapsedPosition = transform.localPosition;
		expandedPosition = collapsedPosition + deltaPosition;

		endPosition = transform.localPosition;
	}
	
	// Expand / Collapse	
	public void StartMoving (Action callback, float duration = DURATION)
	{
		endPosition = expandedPosition;
		onFinish = callback;
		SetStartValues (duration);
	}
	
	public void StartMovingBack (Action callback, float duration = DURATION)
	{
		endPosition = collapsedPosition;
		onFinish = callback;
		SetStartValues (duration);
	}

	float startTime;
	Vector3 startPosition;
	Vector3 changePosition;
	float errorMargin;
	void SetStartValues (float duration)
	{
		startTime = Time.time;
		startPosition = transform.localPosition;
		changePosition = endPosition - startPosition;
		chosenDuration = duration;
		errorMargin = Vector3.Distance (startPosition, endPosition) * .05f;
	}
	
	void Update ()
	{
		if (Vector3.Distance (transform.localPosition, endPosition) > errorMargin) {
			float coeff = (Time.time - startTime) / chosenDuration;
			transform.localPosition = startPosition + coeff * changePosition;

			if (Vector3.Distance (transform.localPosition, endPosition) <= errorMargin) {
				transform.localPosition = endPosition;
				onFinish ();
			}
		}
	}
}
