using UnityEngine;
using System.Collections;

public class BulletExplosion : MonoBehaviour 
{
	static readonly float TIME_TO_LIVE = 5f;
	public float radius;
	public float damage;
	public bool hopAbove;

	[HideInInspector] public PlayerStats stats;


	void Start ()
	{
		DamageAround ();

		if (hopAbove) {
			var pos = transform.position;
			pos.z -= 5f;
			transform.position = pos;
		}

		Destroy (gameObject, TIME_TO_LIVE);
	}
	
	public void Init (PlayerStats stats, float damage, float radius)
	{
		this.stats = stats;
		this.damage = damage;
		this.radius = radius;
	}

	void DamageAround ()
	{
		Collider2D[] around = Physics2D.OverlapCircleAll (transform.position, radius); 
		foreach (Collider2D coll in around)
			if (coll.tag == "Player")
				coll.GetComponent <TankHealth> ().TakeDamage (damage, stats);
			else if (coll.tag == "Destroyable") {
				var barrel = coll.GetComponent <DestroyableBarrel> ();
				
				if (barrel != null)
					barrel.TakeDamage (damage, stats);
				
				// Can also be a bullet, in which case, it explodes by itself, no action needed

			}
	}
}
