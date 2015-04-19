using UnityEngine;
using System.Collections;

public class Explosion : Containable<Explosion>
{
	static readonly float TIME_TO_LIVE = 5f;
	public float 
		radius,
		force,
		damage,
		DoTAmount, // damage over time
		DoTDuration;

	[HideInInspector] public PlayerStats source;

	void Awake ()
	{
		moveToContainer ("Explosions");
	}
	
	void Start ()
	{
		// Hoping above TODO: figure out why particles don't show above sprites
//		var pos = transform.position;
//		pos.z -= 5f;
//		transform.position = pos;

		// We don't destroy this immediately so that the particles may show
		Destroy (gameObject, TIME_TO_LIVE);
	}
	
	public void Setup (PlayerStats source)
	{
		this.source = source;

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
				coll.GetComponent <TankHealth> ().TakeDamage (damage, source);
				var dir = (transform.position - coll.transform.position) * force;
			Debug.Log (name + " is pushing " + coll.name + " for " + dir);
			coll.attachedRigidbody.AddForce (dir); Debug.Log (coll.attachedRigidbody.mass);
				break;

			case "Destroyable":
				var barrel = coll.GetComponent <DestroyableBarrel> ();
				if (barrel != null)
					barrel.TakeDamage (damage, source);
				// Can also be a bullet, in which case, it explodes by itself, no action needed
				break;

			default:
				Debug.LogErrorFormat ("Explosion {0} from {1} tried to damage {2} ({3}) but it shouldn't have", name, source.username, coll.name, coll.tag);
				break;
			}

	}
}
