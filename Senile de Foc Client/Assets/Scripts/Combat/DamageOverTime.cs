using UnityEngine;
using System.Collections;

public class DamageOverTime : Containable<DamageOverTime> 
{
	public float totalDamage;
	public float duration;

	public float particleRate;
	ParticleSystem particles;

	public Damagable affected;
	public PlayerStats source;

	void Awake ()
	{
		moveToContainer ("DoTs");
		particles = Utils.childWithName (transform, "Particles").GetComponent <ParticleSystem> ();
	}

	float lastParticle;
	void Update ()
	{
		if (affected != null) {
			transform.position = affected.transform.position;

			if (Time.time - lastParticle >= particleRate) {
				lastParticle = Time.time;
				particles.Play ();
			}
		}
	}

	public void AttachTo (PlayerStats source, Damagable affected)
	{
		this.source = source;
		this.affected = affected;
		affected.activeDoTs.Add (this);

		maxTicks = (int) (duration / Constants.DOT_RATE);
		damage = totalDamage / maxTicks;
		InvokeRepeating ("ApplyDamage", 0, Constants.DOT_RATE);

		StartCoroutine (Expire ());
	}

	IEnumerator Expire ()
	{
		yield return new WaitForSeconds (duration);
		Clear ();
	}

	float damage;
	int maxTicks, ticks;
	void ApplyDamage ()
	{
		// Extra caution (because the duration does not always divide evenly to the rate)
		if (ticks < maxTicks) {
			affected.TakeDamage (damage, source);
			ticks++;
		}
	}

	public void Clear ()
	{
		Destroy (gameObject);
	}
}
