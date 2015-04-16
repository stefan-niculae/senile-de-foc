using UnityEngine;
using System.Collections;

public class TankWeapon : MonoBehaviour 
{
	[HideInInspector] public float cooldownPeriod;
	[HideInInspector] public GameObject projectilePrefab;
	[HideInInspector] public ParticleSystem fireParticles;

	PlayerStats stats;
	Transform bulletSpawnPos;
	Transform bulletContainer;
	AudioSource audioSource;

	void Awake ()
	{
		audioSource = GetComponent <AudioSource> ();

		// We do this and don't set it public because prefabs can't have non prefab fields preassigned
		bulletContainer = GameObject.Find ("Bullets").transform;

		stats = GetComponentInParent <PlayerStats> ();
		bulletSpawnPos = Utils.childWithName (transform, "Bullet Spawn Position");
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
		spawned.GetComponent <TankBullet> ().Launch (
			Utils.ForwardDirection (stats.barrel.transform),
			stats);
	}
}
