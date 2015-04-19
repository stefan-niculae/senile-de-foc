using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour 
{
	public float fireRate;

	float lastFire;
	bool isReady
	{ 
		get 
		{
			return Time.time - lastFire >= fireRate;
		} 
	}

	AudioSource sound;
	ParticleSystem particles;

	void Fire (bool playSound = true)
	{
		if (isReady) {
			lastFire = Time.time;

			if (playSound)
				sound.Play ();

			particles.Play ();

			ActualFiring ();

		}
	}
	
	public abstract void ActualFiring ();
}
