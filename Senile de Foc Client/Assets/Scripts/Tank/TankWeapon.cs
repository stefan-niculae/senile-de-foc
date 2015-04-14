using UnityEngine;
using System.Collections;

public class TankWeapon : MonoBehaviour 
{
	public TankBarrel barrel;
	public GameObject bulletPrefab;
	public Transform bulletSpawnPos;
	public ParticleSystem fireParticles;
	public float cooldownPeriod;
	
	Transform bulletContainer;

	AudioSource audioSource;

	void Awake ()
	{
		audioSource = GetComponent <AudioSource> ();

		// We do this and don't set it public because prefabs can't have non prefab fields preassigned
		bulletContainer = GameObject.Find ("Bullets").transform;
	}

	float lastSpawn;
	public void Fire (bool playSound = true)
	{
		// If the cooldown period hasn't passed, ignore the call
		if (Time.time - lastSpawn >= cooldownPeriod) {
			lastSpawn = Time.time;

			barrel.Bounce ();
			fireParticles.Play ();
			SpawnBullet ();

			if (playSound)
				audioSource.Play ();
		}
	}

	void SpawnBullet ()
	{
		GameObject spawned = Instantiate (
			bulletPrefab,
			bulletSpawnPos.position,
			barrel.transform.rotation) as GameObject;

		spawned.transform.parent = bulletContainer;
		spawned.GetComponent <TankBullet> ().Launch (Utils.ForwardDirection (barrel.transform));
	}
}
