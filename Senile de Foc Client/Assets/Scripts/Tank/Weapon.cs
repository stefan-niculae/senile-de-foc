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
	AudioSource fireSound;

	void Awake ()
	{
		tankInfo = GetComponentInParent <TankInfo> ();

		OnAwake ();

		fireParticles = Utils.childWithName (effectSpawnPoint, "Fire Particles").GetComponent <ParticleSystem> ();
		fireSound = GetComponent <AudioSource> ();
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

	public void Fire (bool playSound = true)
	{
		if (isReady) {
			lastFire = Time.time;

			SpawnEffect ();

			if (playSound) // TODO: make two sounds, one for bullets, one for special
				fireSound.Play ();

			// The special waves don't have a muzzle flash
			if (fireParticles != null)
				fireParticles.Play ();
		}
	}
	public abstract void OnFire (GameObject spawnedEffect);

	void SpawnEffect ()
	{
		GameObject spawned = Instantiate (
			effectPrefab,
			effectSpawnPoint.position,
			Quaternion.identity) as GameObject;
		OnFire (spawned);
	}
}
