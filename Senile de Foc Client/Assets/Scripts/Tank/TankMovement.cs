using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour 
{
	public float 
		forwardSpeed,
		backwardSpeed,
		rotationSpeed;
	public Transform superParent;

	Rigidbody2D body;
	TankTracksManager tracks;

	void Awake ()
	{
		// Setting the reference
		body = GetComponent <Rigidbody2D> ();
	}

	void Start ()
	{
		// Setting up references
		rot = transform.rotation.eulerAngles;

		// We do this and don't set it public because prefabs can't have non prefab fields preassigned
		tracks = GameObject.Find ("Tracks").GetComponent <TankTracksManager> ();
	}

	void Update ()
	{
		superParent.transform.position = transform.position;
		transform.localPosition = Vector3.zero;
	}

	Vector3 rot;
	public void Move (float horiz, float vert, bool playerInput = false, float targetRot = float.NaN)
	{
		horiz = Mathf.Clamp (horiz, -1f, 1f);
		vert = Mathf.Clamp (vert, -1f, 1f);

		var oldRot = rot.z;
		rot.z += (vert >= 0 ? -horiz : horiz) * rotationSpeed * Time.deltaTime;
		rot.z = rot.z < 0 ? 360f + rot.z : rot.z; // Don't use -20, use 340 instead

		// For rotations from the patroling script
		if (CustomBetween (targetRot, oldRot, rot.z))
			rot.z = targetRot;

		// Apply the newly computed rotation
		transform.rotation = Quaternion.Euler (rot);
		
		var speed = vert > 0 ? forwardSpeed : backwardSpeed;
		var force = vert * Utils.ForwardDirection (transform) * speed * Time.deltaTime;
		body.AddForce (force);
		
		// Only spawn tracks if the tank is moving (and only for the player tank)
		if (playerInput && vert != 0)
			tracks.Show (transform.position, transform.rotation);
	}

	bool CustomBetween (float value, float a, float b)
	{
		return 
			(
				// NaN cancels the script
				!float.IsNaN (value) &&
		    
		    	(
			     // a <= b or b <= a
				 (a <= value && value <= b) ||
				 (b <= value && value <= a)
				)
			);
	}
}
