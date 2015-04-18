using UnityEngine;
using System.Collections;

public class TankBullet : MonoBehaviour 
{
	static readonly float TIME_TO_LIVE = 10f;
	static Transform explosionContainer;

	public float speed;
	public float damage, radius;
	public int maxCollisions; // number of times the bullet can bounce
	public GameObject explosionPrefab;

	[HideInInspector] public PlayerStats stats;
	Rigidbody2D body;
	Collider2D[] colliders;

	void Awake ()
	{
		body = GetComponent <Rigidbody2D> ();
		colliders = GetComponents <Collider2D> ();

		if (explosionContainer == null)
			explosionContainer = GameObject.Find ("Explosions").transform;
	}

	public void Launch (Vector2 direction, PlayerStats stats, Sprite sprite, GameObject explosionPrefab, int bounces, float speed, float damage, float radius)
	{
		body.AddForce (speed * direction);

		this.stats = stats;
		this.speed = speed;
		this.damage = damage;
		this.radius = radius;
		this.explosionPrefab = explosionPrefab;

		GetComponent <SpriteRenderer> ().sprite = sprite;

		// Automatically destroy in a while if the bullet got stuck
		Destroy (gameObject, TIME_TO_LIVE);
	}

	int timesCollided;
	Vector3 pointOfCollision;
	void OnCollisionEnter2D (Collision2D collision) 
	{
		// We use this because by the time the explode function starts executing the bullet
		// is already away
		pointOfCollision = collision.contacts [0].point;
		pointOfCollision.z = transform.position.z;

		// Bounces off walls
		if (collision.gameObject.tag == "World") {
			timesCollided++;
			if (timesCollided == maxCollisions)
				Explode ();
			else {
				// We first disable colliders so that the collision doesn't register twice
				// (once with the head and again with the tail)
				StartCoroutine (DisableCollidersABit ());
				RotateToVelocity ();
			}
		}

		// Explodes on players and barrels
		else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Destroyable")
			Explode ();

		else
			Debug.LogErrorFormat ("Hit something that shouldn't be hittable, {0} ({1})", collision.gameObject.name, collision.gameObject.tag);
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
			pointOfCollision,
			Quaternion.identity) as GameObject;
		explosion.transform.parent = explosionContainer;

		explosion.GetComponent <BulletExplosion> ().Init (stats, damage, radius);
		Destroy (gameObject);
	}
}
