using UnityEngine;
using System.Collections;

public class Explosion : Containable<Explosion>
{
	static readonly float TIME_TO_LIVE = 5f;
	public float 
		radius,
		force,
		damage;
	public GameObject DoTPrefab;

	[HideInInspector] public TankInfo source;

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
	
	public void Setup (TankInfo source, Damagable ignore = null)
	{
		this.source = source;

		DamageAround (ignore);
	}

	void DamageAround (Damagable ignore)
	{
		Collider2D[] around = Physics2D.OverlapCircleAll (transform.position, radius);

		foreach (Collider2D coll in around) {
			var damagable = coll.GetComponent <Damagable> ();
			if (damagable != null && damagable != ignore) {

				damagable.TakeDamage (damage, source);

				var dir = (coll.transform.position - transform.position) * force;
				damagable.GetPushed (dir);

				ApplyDoT (source, damagable);
			}
		}
	}

	void ApplyDoT (TankInfo source, Damagable affected)
	{
		if (DoTPrefab != null) {
			var DoT = (Instantiate (DoTPrefab) as GameObject).GetComponent <DamageOverTime> ();
			DoT.AttachTo (source, affected);
		}
	}
}
