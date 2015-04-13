using UnityEngine;
using System.Collections;

public class TankWeapon : MonoBehaviour 
{
	public TankBarrel barrel;
	public GameObject bulletPrefab;
	public Transform bulletSpawnPos;
	public ParticleSystem fireParticles;

	Transform bulletContainer;

	public float cooldownPeriod;

	TankMovement movement;
	AudioSource audioSource;

	void Awake ()
	{
		movement = GetComponent <TankMovement> ();
		audioSource = GetComponent <AudioSource> ();

		// We do this and don't set it public because prefabs can't have non prefab fields preassigned
		bulletContainer = GameObject.Find ("Bullets").transform;
	}

	float lastSpawn;

	public void Fire (bool playSound = true)
	{
		// If the cooldown period hasn't passed, ignore the call
		if (Time.time - lastSpawn < cooldownPeriod)
			return;
		lastSpawn = Time.time;

		barrel.Bounce ();
		fireParticles.Play ();
		SpawnBullet ();

		if (playSound)
			audioSource.Play ();
	}

	void SpawnBullet ()
	{
		GameObject spawned = Instantiate (
			bulletPrefab,
			bulletSpawnPos.position,
			transform.rotation) as GameObject;

		spawned.transform.parent = bulletContainer;
		spawned.GetComponent <TankBullet> ().Launch (movement.ForwardDirection ());
	}
}
