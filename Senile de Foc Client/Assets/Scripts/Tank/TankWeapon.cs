using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankWeapon : MonoBehaviour 
{
	public float fireRate;
	public GameObject projectilePrefab;
	ParticleSystem fireParticles;

	PlayerStats stats;
	Transform bulletSpawnPos;
	AudioSource audioSource;
	Image cooldownFill;

	void Awake ()
	{
		audioSource = GetComponent <AudioSource> ();

		stats = GetComponentInParent <PlayerStats> ();
		bulletSpawnPos = Utils.childWithName (transform, "Bullet Spawn Position");
		fireParticles = Utils.childWithName (bulletSpawnPos, "Fire Particles").GetComponent <ParticleSystem> ();

		if (stats.controlledPlayer)
			cooldownFill = GameObject.Find ("Projectile Cooldown Fill").GetComponent <Image> ();
	}

	void Update ()
	{
		// Only not-null when this is the controlled player
		if (cooldownFill != null) 
			cooldownFill.fillAmount = 1f - (Time.time - lastSpawn) / fireRate;

	}

	float lastSpawn;
	public void Fire (bool playSound = true)
	{
		// If the cooldown period hasn't passed, ignore the call
		if (Time.time - lastSpawn >= fireRate) {
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

		spawned.GetComponent <Projectile> ().Launch (
			Utils.ForwardDirection (stats.barrel.transform),
			stats);
	}
}
