using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Secondary : Weapon
{
	public override void OnAwake ()
	{
		effectSpawnPoint = transform;
		fireRate = Constants.SECONDARY_FIRE_RATE;
	}

	public override void OnFire (GameObject spawnedEffect)
	{
		spawnedEffect.GetComponent <Explosion> ().Setup (tankInfo, ignore: tankInfo.health);
	}
}
