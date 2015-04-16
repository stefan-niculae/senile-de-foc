using UnityEngine;
using System.Collections;

public class TankBullet : MonoBehaviour 
{
	static readonly float TIME_TO_LIVE = 10f;

	public float speed;
	public float damage, radius;

	// Number of times the bullet can bounce
	public int maxCollisions;
	public GameObject explosionPrefab;

	[HideInInspector] public PlayerStats stats;

	int timesCollided;

	Rigidbody2D body;
	Collider2D[] colliders;

	void Awake ()
	{
		body = GetComponent <Rigidbody2D> ();
		colliders = GetComponents <Collider2D> ();
	}

	public void Launch (Vector2 direction, PlayerStats stats, int bounces, float speed, float damage, float radius)
	{
		body.AddForce (speed * direction);

		this.stats = stats;
		this.speed = speed;
		this.damage = damage;
		this.radius = radius;

		// Automatically destroy in a while if the bullet got stuck
		Destroy (gameObject, TIME_TO_LIVE);
	}

	void OnCollisionEnter2D (Collision2D collision) 
	{
		// Bounces off walls
		if (collision.gameObject.tag == "World") {
			timesCollided++;
			if (timesCollided == maxCollisions)
				Explode ();

			// We first disable colliders so that the collision doesn't register twice
			// (once with the head and again with the tail)
			StartCoroutine (DisableCollidersABit ());
			RotateToVelocity ();
		}

		// Explodes on players
		else if (collision.gameObject.tag == "Player") 
			Explode ();
	}
	
	IEnumerator DisableCollidersABit ()
	{
		foreach (Collider2D collider in colliders)
			collider.enabled = false;
		
		yield return new WaitForSeconds (.05f);
		
		foreach (Collider2D collider in colliders)
			collider.enabled = true;
	}
	
	void RotateToVelocity() 
	{ 
		// Get the position of where the bullet will be in an instant
		var slightlyForward = (Vector2)transform.position + (Vector2)body.velocity;

		// Rotate towards that direction
		transform.rotation = Utils.LookAt2D (transform, slightlyForward);
	}

	void Explode ()
	{
		GameObject explosion = Instantiate (
			explosionPrefab,
			transform.position,
			Quaternion.identity) as GameObject;
		explosion.transform.parent = GameObject.Find ("Explosions").transform;

		explosion.GetComponent <BulletExplosion> ().Init (stats, damage, radius);

		Destroy (gameObject);
	}
}
