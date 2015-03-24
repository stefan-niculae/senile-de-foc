using UnityEngine;
using System.Collections;

public class TankBarrel : MonoBehaviour 
{
	static readonly float EPSILON = .0001f;

	Vector3 initialPos;
	[HideInInspector] public Vector3 firePos;
	public float 
		fireOffset,
		backwardSpeed,
		forwardSpeed;

	void Awake ()
	{
		initialPos = transform.localPosition;
		firePos = initialPos;
		firePos.y -= fireOffset;
	}

	public void Bounce ()
	{
		transform.localPosition = initialPos;
		goingBack = true;
	}

	bool goingBack = false;
	bool goingFront = false;

	void Update ()
	{
		if (goingBack) {
			transform.localPosition = Vector3.MoveTowards (
				transform.localPosition,
				firePos,
				backwardSpeed * Time.deltaTime);

			if (Vector2.Distance (transform.localPosition, firePos) < EPSILON) {
				goingBack = false;
				goingFront = true;
			}
		}

		if (goingFront) {
			transform.localPosition = Vector3.MoveTowards (
				transform.localPosition,
				initialPos,
				forwardSpeed * Time.deltaTime);

			if (Vector2.Distance (transform.localPosition, initialPos) < EPSILON)
				goingFront = false;

		}

	}
}
