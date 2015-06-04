using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour 
{
	public float 
		forwardSpeed,
		backwardSpeed,
		rotationSpeed;

	Rigidbody2D body;
	[HideInInspector] public TankTracksManager tracks;

	void Awake ()
	{
		// Setting the reference
		body = GetComponent <Rigidbody2D> ();
	}

	public void Move (float horiz, float vert, bool playerInput = true, float targetRot = float.NaN)
	{
		if (horiz != 0 || vert != 0) {

			float self = transform.eulerAngles.z;
			float input = AxisToRotation (horiz, vert);
			if (self != 0)
				self = 360 - self;
			
			float diff = Mathf.Abs (input - self);
			if (diff > 180)
				diff = 360 - diff;
			
			bool forwards = diff < 120;


			if (!forwards) {
				input += 180;
				input %= 360;
			}
			transform.rotation = Quaternion.RotateTowards (
				transform.rotation,
				Quaternion.Euler (new Vector3 (0, 0, 360 - input)),
				rotationSpeed
			);

			float speed = forwards ? forwardSpeed : backwardSpeed;
			int sens = forwards ? 1 : -1;
			var force = sens *  Utils.ForwardDirection (transform) * speed * Time.deltaTime;
			body.AddForce (force);
		
			// Only spawn tracks if the tank is moving (and only for the player tank)
			tracks.Show (transform.position, transform.rotation);
			// TODO engine start sound, running sound
			
		}
	}

	float AxisToRotation (float x, float y)
	{
		Vector2 projected = new Vector2 (x * Mathf.Sqrt (1 - y * y / 2),
										 y * Mathf.Sqrt (1 - x * x / 2));

		// For help, checkout the Movement Illustrator
		float rot = Mathf.Atan2 (projected.x,
			   			  	     projected.y);
		rot *= Mathf.Rad2Deg;

		if (rot < 0)
			rot += 360;

		return rot;
	}

}
