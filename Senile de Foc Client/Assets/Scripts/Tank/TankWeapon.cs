using UnityEngine;
using System.Collections;

public class TankWeapon : MonoBehaviour 
{
	public TankBarrel barrel;
	public GameObject bulletPrefab;
	public Transform bulletContainer;
	public Transform bulletSpawnPos;

	public float cooldownPeriod;

	TankMovement movement;
	AudioSource audioSource;

	void Awake ()
	{
		movement = GetComponent <TankMovement> ();
		audioSource = GetComponent <AudioSource> ();
	}

	void Update ()
	{
		if (Input.GetButton ("Fire1"))
			Fire ();
	}
	
	float lastSpawn;

	void Fire  ()
	{
		// If the cooldown period hasn't passed, ignore the call
		if (Time.time - lastSpawn < cooldownPeriod)
			return;
		lastSpawn = Time.time;

		barrel.Bounce ();
		SpawnBullet ();
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
