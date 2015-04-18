using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankWeapon : MonoBehaviour 
{
	public float cooldownPeriod;
	public GameObject projectilePrefab;
	public ParticleSystem fireParticles;

	public int projectileBounces;
	public float projectileSpeed;
	public Sprite projectileSprite;
	public float explosionDamage;
	public float explosionRadius;
	public GameObject explosionPrefab;

	static Transform bulletContainer;
	PlayerStats stats;
	Transform bulletSpawnPos;
	AudioSource audioSource;
	Image cooldownFill;

	void Awake ()
	{
		audioSource = GetComponent <AudioSource> ();

		// We do this and don't set it public because prefabs can't have non prefab fields preassigned
		if (bulletContainer == null)
			bulletContainer = GameObject.Find ("Bullets").transform;

		stats = GetComponentInParent <PlayerStats> ();
		bulletSpawnPos = Utils.childWithName (transform, "Bullet Spawn Position");

		if (stats.controlledPlayer)
			cooldownFill = GameObject.Find ("Projectile Cooldown Fill").GetComponent <Image> ();
	}

	void Update ()
	{
		// Only not-null when this is the controlled player
		if (cooldownFill != null) 
			cooldownFill.fillAmount = 1f - (Time.time - lastSpawn) / cooldownPeriod;

	}

	float lastSpawn;
	public void Fire (bool playSound = true)
	{
		// If the cooldown period hasn't passed, ignore the call
		if (Time.time - lastSpawn >= cooldownPeriod) {
			lastSpawn = Time.time;

			stats.barrel.Bounce ();
			fireParticles.Play ();
			SpawnProjectile ();

			if (playSound)
				audioSource.Play ();
		}
	}

	void SpawnProjectile ()
	{
		GameObject spawned = Instantiate (
			projectilePrefab,
			bulletSpawnPos.position,
			stats.barrel.transform.rotation) as GameObject;

		spawned.transform.parent = bulletContainer;
		spawned.GetComponent <TankProjectile> ().Launch (
			Utils.ForwardDirection (stats.barrel.transform),
			stats,
			projectileSprite,
			explosionPrefab,
			projectileBounces,
			projectileSpeed,
			explosionDamage,
			explosionRadius);
	}
}
