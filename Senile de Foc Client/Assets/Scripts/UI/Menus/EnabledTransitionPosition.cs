using UnityEngine;
using System.Collections;
using System;

public class EnabledTransitionPosition : MonoBehaviour 
{
	static readonly float DURATION = .25f;
	Vector3 expandedPosition;
	Vector3 collapsedPosition;
	public Vector3 deltaPosition;


	Action onFinish;

	
	Vector3 endPosition;
	void Start ()
	{
		collapsedPosition = transform.localPosition;
		expandedPosition = collapsedPosition + deltaPosition;

		endPosition = transform.localPosition;
	}
	
	// Expand / Collapse	
	public void StartExpanding (Action callback = null)
	{
		endPosition = expandedPosition;
		onFinish = callback;
		SetStartValues ();
	}
	
	public void StartCollapsing (Action callback)
	{
		endPosition = collapsedPosition;
		onFinish = callback;
		SetStartValues ();
	}

	float startTime;
	Vector3 startPosition;
	Vector3 changePosition;
	void SetStartValues ()
	{
		startTime = Time.time;
		startPosition = transform.localPosition;
		changePosition = endPosition - startPosition;
	}

	static readonly float ERROR_MARGIN = 7.5f;
	void Update ()
	{
		if (Vector3.Distance (transform.localPosition, endPosition) > ERROR_MARGIN) {
			float coeff = (Time.time - startTime) / DURATION;
			transform.localPosition = startPosition + coeff * changePosition;

			if (Vector3.Distance (transform.localPosition, endPosition) <= ERROR_MARGIN) {
				transform.localPosition = endPosition;
				onFinish ();
			}
		}
	}


}
