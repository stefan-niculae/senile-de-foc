using UnityEngine;
using System.Collections;

public class TankBarrel : MonoBehaviour 
{
	static readonly float EPSILON = .0001f;

	public float 
		fireOffset,
		backwardSpeed,
		forwardSpeed;

	Vector3 restPos = new Vector3 (0, 0, -.2f);
	Vector3 firePos;

	public void Bounce ()
	{
		transform.localPosition = restPos;
		goingBack = true;

		float rot = transform.localRotation.eulerAngles.z * Mathf.Deg2Rad;
		Vector3 coeffs = new Vector3 (Mathf.Sin (rot), -Mathf.Cos (rot));
		firePos = restPos + fireOffset * coeffs;
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
				restPos,
				forwardSpeed * Time.deltaTime);
			
			if (Vector2.Distance (transform.localPosition, restPos) < EPSILON)
				goingFront = false;
			
		}
	}

	public void Rotate (Quaternion rot)
	{
		// reenable?
//		// We do this because the barrel should not rotate around its axis
//		// instead, it should rotate around the barrel stand
//		stand.rotation = rot;
		transform.rotation = rot;
	}
}
