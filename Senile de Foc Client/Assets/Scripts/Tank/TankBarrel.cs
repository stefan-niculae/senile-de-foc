using UnityEngine;
using System.Collections;

public class TankBarrel : MonoBehaviour 
{
	static readonly float EPSILON = .0001f;

	public float 
		fireOffset,
		backwardSpeed,
		forwardSpeed;

	Transform stand;
	Vector3 initialPos;
	[HideInInspector] public Vector3 firePos;

	void Awake ()
	{
		stand = transform.parent;
		initialPos = transform.localPosition;
		firePos = initialPos;
		firePos.y -= fireOffset;
	}

	public void Bounce ()
	{
		transform.localPosition = initialPos;
		goingBack = true;
	}

	void Update ()
	{
		HandleBouncing ();
	}
	
	bool goingBack;
	bool goingFront;
	void HandleBouncing ()
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

	public void Rotate (Quaternion rot)
	{
		// We do this because the barrel should not rotate around its axis
		// instead, it should rotate around the barrel stand
		stand.rotation = rot;
	}
}
