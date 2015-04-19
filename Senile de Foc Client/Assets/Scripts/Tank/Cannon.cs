using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cannon : Weapon 
{
    public override void OnAwake ()
	{
		effectSpawnPoint = Utils.childWithName (transform, "Bullet Spawn Position");

		if (stats.controlledPlayer)
			cooldownFill = GameObject.Find ("Projectile Cooldown Fill").GetComponent <Image> ();
	}

	public override void OnFire (GameObject spawnedProjectile)
	{
		stats.barrel.Bounce ();

		spawnedProjectile.transform.rotation = stats.barrel.transform.rotation;
		spawnedProjectile.GetComponent <Projectile> ().Launch (
			Utils.ForwardDirection (stats.barrel.transform),
			stats);
	}
}
