using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Damagable : MonoBehaviour 
{
	protected float maxHp = 100;
	protected float respawnTime;
	public float damageAbsorbtion;

	[HideInInspector] public int networkID = NetworkConstants.NOT_SET;

	[HideInInspector] public HealthBar bar;
	public GameObject explosionPrefab;

	protected ThresholdParticle[] damaged;

	public List<DamageOverTime> activeDoTs;

	float _amount;
	public float amount
	{
		get
		{ return _amount; }
		
		set 
		{
			_amount = Mathf.Clamp (value, 0, maxHp);

			// Barrels don't have a health bar
			if (bar != null)
				bar.Display (_amount, maxHp);

			if (amount == 0)
				ZeroHealth ();
		}
	}

	Vector3 deathPosition;
	void ZeroHealth ()
	{
		deathPosition = transform.position;
		// Teleport the body far away
		transform.position = hidden;

		StartCoroutine (Respawn ());

		// Clear DoTs (they do not transfer after death)
		foreach (var DoT in activeDoTs)
			DoT.Clear ();
		activeDoTs.Clear ();

		OnZeroHealth ();
	}
	public abstract void OnZeroHealth ();

	void Awake ()
	{
		amount = maxHp;
		OnAwake ();

		body = GetComponent <Rigidbody2D> ();
		hidden = Constants.HIDDEN;
		hidden.z = transform.position.z;

		activeDoTs = new List<DamageOverTime> ();

		if (networkID != NetworkConstants.NOT_SET && tag != "Player") {
			if (networkID < 5)
				Debug.LogFormat ("on awake registering {0} with id {1}", name, networkID);
			RegisterThis (networkID);
		}
	}
	public abstract void OnAwake ();

	protected void RegisterThis (int networkID)
	{
		try {
			// I don't know why the gameserver script's awake runs after the damagable's awake
			// I suspect it's because damagable has some infiltrations in scripts that run in edit mode
			if (GameServer.Instance.damageables == null)
				GameServer.Instance.damageables = new Dictionary<int, Damagable> ();
			GameServer.Instance.damageables.Add (networkID, this);
		} catch (ArgumentException argEx) {
			Debug.LogWarningFormat ("Network ID {0} is already in the damagables dictionary\n{2}", networkID, argEx);
		}
	}

	void Start ()
	{
		OnStart ();

		StartCoroutine (Respawn (firstSpawn: true));
	}
	public abstract void OnStart ();

	protected float minWait, maxWait;
	float lastPlay, toWait;
	void Update ()
	{
		OnUpdate ();

		// If enough time has passed since the last particle play
		if (Time.time - lastPlay >= toWait) {

			lastPlay = Time.time;
			toWait = UnityEngine.Random.Range (minWait, maxWait);

			for (int i = damaged.Length - 1; i >= 0; i--)
				if (amount <= damaged [i].threshold) {
					damaged [i].randParticle.Play ();
					break;
				}
		}
	}
	public abstract void OnUpdate ();

	bool alreadyExploded;
	public void TakeDamage (float damage, TankInfo source)
	{

		// Apply the absorbtion
		damage *= 1 - damageAbsorbtion;

		OnTakingDamage (damage, source);
		
		// Apply the damage
		amount -= damage;

		GameServer.Instance.SendHealthUpdate (networkID, amount);

		// If it has killed the target
		if (amount <= 0 && !alreadyExploded) {
			alreadyExploded = true;
			Die (source);
		}
	}
	public abstract void OnTakingDamage (float damage, TankInfo source);	

	Rigidbody2D body;
	public void GetPushed (Vector2 dir)
	{
		body.AddForce (dir);
	}

	Vector3 hidden = Constants.HIDDEN;
	void Die (TankInfo source)
	{
		OnDeath (source);
		Explode (source);
	}
	public abstract void OnDeath (TankInfo source);

	void Explode (TankInfo source)
	{
		alreadyExploded = true;

		GameObject explosion = Network.Instantiate (
			explosionPrefab,
			deathPosition,
			Quaternion.identity,
			0) as GameObject;

		explosion.GetComponent <Explosion> ().Setup (source, ignore: this);
	}
	
	public IEnumerator Respawn (bool firstSpawn = false)
	{
		if (!firstSpawn)
			yield return new WaitForSeconds (respawnTime);

		OnRespawn (firstSpawn);

		// Stats
		amount = maxHp;
		alreadyExploded = false;

		// Stopping every damaged particle effects
		foreach (var d in damaged)
			foreach (var p in d.particles)
				p.Stop ();
	}
	public abstract void OnRespawn (bool firstSpawn);
}

public class ThresholdParticle 
{
	public float threshold;
	public ParticleSystem[] particles;

	float minRot, maxRot;

	Transform particleParent;
	public ParticleSystem randParticle
	{
		get
		{
			if (!float.IsNaN (minRot))
				particleParent.rotation = Utils.random2DRotation (minRot, maxRot);
			return Utils.randomFrom (particles);
		}
	}

	public ThresholdParticle (float threshold, Transform parent, float minRot = float.NaN, float maxRot = float.NaN)
	{
		this.threshold = threshold;
		this.minRot = minRot;
		this.maxRot = maxRot;

		int n = parent.childCount;
		particles = new ParticleSystem[n];
		for (int i = 0; i < n; i++)
			particles [i] = parent.GetChild (i).GetComponent <ParticleSystem> ();

		particleParent = particles [0].transform.parent;
	}
}
