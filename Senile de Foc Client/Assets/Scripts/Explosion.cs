using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	static readonly float TIME_TO_LIVE = 5f;
	float 
		radius,
		force,
		damage,
		DoTAmount, // damage over time
		DoTDuration;

	[HideInInspector] public PlayerStats stats;
	
	void Start ()
	{
		// Hoping above TODO: figure out why particles don't show above sprites
//		var pos = transform.position;
//		pos.z -= 5f;
//		transform.position = pos;

		// We don't destroy this immediately so that the particles may show
		Destroy (gameObject, TIME_TO_LIVE);
	}
	
	public void Setup (PlayerStats stats, float radius, float force, float damage, float DoTAmount, float DoTDuration, float delay = 0)
	{
		this.stats = stats;
		this.radius = radius;
		this.force = force;
		this.damage = damage;
		this.DoTAmount = DoTAmount;
		this.DoTDuration = DoTDuration;

		StartCoroutine (WaitAndDamageAround (delay));
	}

	IEnumerator WaitAndDamageAround (float time)
	{
		yield return new WaitForSeconds (time);
		DamageAround ();
	}

	void DamageAround ()
	{
		Collider2D[] around = Physics2D.OverlapCircleAll (transform.position, radius); 

		foreach (Collider2D coll in around) 
			switch (coll.tag) {
			case "World":
				// Unbreakables
				break;

			case "Player":
				coll.GetComponent <TankHealth> ().TakeDamage (damage, stats);
				break;

			case "Destroyable":
				var barrel = coll.GetComponent <DestroyableBarrel> ();
				if (barrel != null)
					barrel.TakeDamage (damage, stats);
				// Can also be a bullet, in which case, it explodes by itself, no action needed
				break;

			default:
				Debug.LogErrorFormat ("Explosion {0} from {1} tried to damage {2} ({3}) but it shouldn't have", name, stats.username, coll.name, coll.tag);
				break;
			}

	}
}
