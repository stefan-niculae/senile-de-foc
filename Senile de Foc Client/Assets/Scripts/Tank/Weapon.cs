using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour 
{
	public float fireRate;
	public GameObject effectPrefab; // projectile (missle, bullet) or special power (electricity, blast, gust, poison)

	protected TankInfo tankInfo;
	protected Transform effectSpawnPoint; // Don't forget to set this OnAwake!!
	public Image cooldownFill;
	ParticleSystem fireParticles;

	void Awake ()
	{
		tankInfo = GetComponentInParent <TankInfo> ();

		OnAwake ();

		fireParticles = Utils.childWithName (effectSpawnPoint, "Fire Particles").GetComponent <ParticleSystem> ();
	}
	public abstract void OnAwake ();

	void Update ()
	{
		// Only not-null when this is the controlled player
		if (cooldownFill != null) 
			cooldownFill.fillAmount = 1f - (Time.time - lastFire) / fireRate;
	}

	float lastFire;
	bool isReady
	{ 
		get 
		{
			return Time.time - lastFire >= fireRate;
		} 
	}

	public void Fire ()
	{
		if (isReady) {
			lastFire = Time.time;

			SpawnEffect ();

			// The special waves don't have a muzzle flash so this is set to null
			if (fireParticles != null)
				fireParticles.Play ();
		}
	}
	public abstract void OnFire (GameObject spawnedEffect);

	void SpawnEffect ()
	{
		GameObject spawned = Network.Instantiate (
			effectPrefab,
			effectSpawnPoint.position,
			Quaternion.identity,
			0) as GameObject;
		OnFire (spawned);
	}
}
