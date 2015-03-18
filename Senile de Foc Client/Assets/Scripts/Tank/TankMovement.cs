using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour 
{
	public float 
		forwardSpeed,
		backwardSpeed,
		rotationSpeed;
	Rigidbody2D body;

	public TankTracks tracks;

	void Awake ()
	{
		// Setting the reference
		body = GetComponent <Rigidbody2D> ();
	}

	void Start ()
	{
		// Setting up references
		rot = transform.rotation.eulerAngles;
	}

	float horiz, vert, angle, speed;
	Vector3 rot, force;

	void Update () 
	{
		horiz 	= Input.GetAxis ("Horizontal");
		vert 	= Input.GetAxis ("Vertical");

		rot.z += (vert >= 0 ? -horiz : horiz) * rotationSpeed;
		transform.rotation = Quaternion.Euler (rot);

		angle = (transform.eulerAngles.magnitude + 90f) * Mathf.Deg2Rad;

		speed = vert > 0 ? forwardSpeed : backwardSpeed;
		force = speed * vert * new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
		body.AddForce (force);

		// Only spawn tracks if the tank is moving
		if (vert != 0)
			tracks.Show (transform.position, transform.rotation);
	}
}
