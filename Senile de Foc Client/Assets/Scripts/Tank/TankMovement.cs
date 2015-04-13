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
	public void Move (float horiz, float vert, bool playerInput, float targetRot = float.NaN)
	{
		horiz = Mathf.Clamp (horiz, -1f, 1f);
		vert = Mathf.Clamp (vert, -1f, 1f);

		var oldRot = rot.z;
		rot.z += (vert >= 0 ? -horiz : horiz) * rotationSpeed * Time.deltaTime;
		rot.z = rot.z < 0 ? 360f + rot.z : rot.z; // Don't use -20, use 340 instead

		// If we have a target (from an automated patroling script)
		if (!float.IsNaN (targetRot) &&
		    // And the target is between the old and the current value
		    (	
		 		(oldRot <= targetRot && targetRot <= rot.z) ||
		    	(rot.z <= targetRot && targetRot <= oldRot))
		    )
			rot.z = targetRot;

		transform.rotation = Quaternion.Euler (rot);
		
		var speed = vert > 0 ? forwardSpeed : backwardSpeed;
		var force = vert * ForwardDirection () * speed * Time.deltaTime;
		body.AddForce (force);
		
		// Only spawn tracks if the tank is moving (and only for the player tank)
		if (playerInput && vert != 0)
			tracks.Show (transform.position, transform.rotation);
	}

	public Vector2 ForwardDirection ()
	{
		var angle = (transform.eulerAngles.magnitude + 90f) * Mathf.Deg2Rad;
		return new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
	}

}
