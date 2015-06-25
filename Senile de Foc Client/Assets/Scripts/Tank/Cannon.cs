using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cannon : Weapon 
{
    public override void OnAwake ()
	{
		effectSpawnPoint = Utils.childWithName (transform, "Bullet Spawn Position");
	}

	public override void OnFire (GameObject spawnedProjectile)
	{
		tankInfo.barrel.Bounce ();

		spawnedProjectile.transform.rotation = tankInfo.barrel.transform.rotation;
		spawnedProjectile.GetComponent <Projectile> ().Launch (
			Utils.ForwardDirection (tankInfo.barrel.transform),
			tankInfo);
	}
}
