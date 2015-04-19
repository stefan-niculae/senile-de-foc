using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Secondary : Weapon
{
	public override void OnAwake ()
	{
		effectSpawnPoint = transform;
		
		if (stats.controlledPlayer)
			cooldownFill = GameObject.Find ("Secondary Cooldown Fill").GetComponent <Image> ();

		fireRate = 1.5f;
	}

	public override void OnFire (GameObject spawnedEffect)
	{
		spawnedEffect.GetComponent <Explosion> ().Setup (stats, stats.health);
	}
}
