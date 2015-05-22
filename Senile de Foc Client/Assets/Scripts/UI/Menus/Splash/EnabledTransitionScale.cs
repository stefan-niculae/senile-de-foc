using UnityEngine;
using System.Collections;
using System;

public class EnabledTransitionScale : MonoBehaviour 
{
	public static readonly float DURATION = .25f;

	public bool initiallyShrunken;
	public Vector3 shrunkenScale;
	public Vector3 grownScale;

	Action onFinish;
	
	Vector3 destinationScale;
	void Start ()
	{
		if (initiallyShrunken)
			transform.localScale = shrunkenScale;
		destinationScale = transform.localScale;
	}

	// Shrink / Grow
	public void StartShrinking (Action callback)
	{
		destinationScale = shrunkenScale;
		onFinish = callback;
		SetStartValues ();
	}

	public void StartGrowing (Action callback)
	{
		destinationScale = grownScale;
		onFinish = callback;
		SetStartValues ();
	}


	float startTime;
	Vector3 startScale;
	Vector3 changeScale;
	void SetStartValues ()
	{
		startTime = Time.time;
		startScale = transform.localScale;
		changeScale = destinationScale - startScale;
	}

	void Update () 
	{
		if (Vector3.Distance (transform.localScale, destinationScale) > Constants.MED_EPSILON) {
			float coeff = (Time.time - startTime) / DURATION;
			transform.localScale = startScale + coeff * changeScale;

			if (Vector3.Distance (transform.localScale, destinationScale) <= Constants.MED_EPSILON) {
				transform.localScale = destinationScale;
				onFinish ();
			}
		}

	}
	
}
