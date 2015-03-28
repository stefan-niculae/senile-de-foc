using UnityEngine;
using System.Collections;

public class TankBullet : MonoBehaviour 
{
	public float speed;
	public int maxCollisions; // before the bullet disappears
	int timesCollided = 2; // bugs out on more :/

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

	void OnCollisionEnter2D (Collision2D collision) 
	{
		if (collision.gameObject.tag == "World") {
			timesCollided++;
			if (timesCollided == maxCollisions)
				Disappear ();

			if (collision.contacts.Length > 1)
				Debug.LogErrorFormat ("{} hit has more than one contact point ({})", name, collision.contacts.Length);

			// Disable the colliders for a bit because otherwise the collision would register twice
			StartCoroutine (DisableCollidersABit ());

			// Rotating the bullet according to the angle it hit the obstacle
			ContactPoint2D contact = collision.contacts[0];
			float angle = Vector2.Angle (body.velocity, contact.normal);
			transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -angle));
		}
		else if (collision.gameObject.tag == "Player") {
			Debug.Log ("Hit a player (" + collision.gameObject.name + ")!");
		}
	}

	void Disappear ()
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
