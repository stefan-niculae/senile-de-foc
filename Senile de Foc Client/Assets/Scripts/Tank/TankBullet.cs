using UnityEngine;
using System.Collections;

public class TankBullet : MonoBehaviour 
{
	public float speed;
	public int maxCollisions; // before the bullet disappears
	int timesCollided = 0;

	Rigidbody2D body;
	Collider2D[] colliders;

	void Awake ()
	{
		body = GetComponent <Rigidbody2D> ();
		colliders = GetComponents <Collider2D> ();
	}

	public void Launch (Vector2 direction)
	{
		body.AddForce (speed * direction);
	}

	Vector3 rot;

	void OnCollisionEnter2D (Collision2D collision) 
	{
		if (collision.gameObject.tag == "World") {
			timesCollided++;
			if (timesCollided == maxCollisions)
				Explode ();

			RotateToVelocity ();
		}
		else if (collision.gameObject.tag == "Player") {
			Debug.Log ("Hit a player (" + collision.gameObject.name + ")!");
			Explode ();
		}
	}

	void RotateToVelocity() 
	{ 
		Vector3 slightlyForward = (Vector3)transform.position + (Vector3)body.velocity;

		transform.LookAt (slightlyForward);
		transform.rotation = Rot3Dto2D (transform.rotation);
	}

	Quaternion Rot3Dto2D (Quaternion rotation)
	{
		rot = rotation.eulerAngles;
		rot.z = rot.x + rot.y;
		rot.y = rot.x = 0;
		rot.z = rot.z % 360;

		if (rot.z > 180f)
			rot.z -= 180f;
		else
			rot.z = -rot.z;

		return Quaternion.Euler (rot);
	}

	void Explode ()
	{
		// TODO: add a puff!
		Destroy (gameObject);
	}

	IEnumerator DisableCollidersABit ()
	{
		foreach (Collider2D collider in colliders)
			collider.enabled = false;

		yield return new WaitForSeconds (.1f);

		foreach (Collider2D collider in colliders)
			collider.enabled = true;
	}
}
