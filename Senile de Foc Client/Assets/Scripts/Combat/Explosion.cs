using UnityEngine;
using System.Collections;

public class Explosion : Containable<Explosion>
{
	static readonly float TIME_TO_LIVE = 5f;
	public ExplosionStats stats;
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
		if (GetComponent <NetworkView> ().isMine)
			Utils.NetworkDestroy (this, gameObject, TIME_TO_LIVE);
	}

	public void Setup (TankInfo source, ExplosionStats stats = null, Damagable ignore = null)
	{
		this.source = source;
		if (stats != null)
			this.stats = stats;
		DamageAround (ignore: ignore);
	}

	void DamageAround (Damagable ignore)
	{
		Collider2D[] around = Physics2D.OverlapCircleAll (transform.position, stats.radius);

		foreach (Collider2D coll in around) {
			var damagable = coll.GetComponent <Damagable> ();
			if (damagable != null && damagable != ignore) {

				damagable.TakeDamage (stats.damage, source.playerInfo.orderNumber);

				var dir = (coll.transform.position - transform.position) * stats.force;
				damagable.GetPushed (dir);

				ApplyDoT (source, damagable);
			}
		}
	}

	void ApplyDoT (TankInfo source, Damagable affected)
	{
		if (DoTPrefab != null) {
			var DoT = (Network.Instantiate (DoTPrefab, Vector3.zero, Quaternion.identity, 0) as GameObject).GetComponent <DamageOverTime> ();
			DoT.AttachTo (source, affected);
		}
	}
}

[System.Serializable]
public class ExplosionStats
{
	public float damage;
	public float radius;
	public float force;

	public override string ToString ()
	{
		return string.Format ("Damage: {0}, Radius: {1}, Force: {2}", damage, radius, force);
	}
}
