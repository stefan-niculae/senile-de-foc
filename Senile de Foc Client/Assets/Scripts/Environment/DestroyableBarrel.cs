using UnityEngine;
using System.Collections;

public class DestroyableBarrel : Damagable
{
	public override void OnAwake ()
	{
		maxHp = 25;
		respawnTime = 5;

		damaged = new ThresholdParticle[1];
		damaged [0] = new ThresholdParticle (maxHp / 2f, Utils.childWithName (transform, "Damaged Barrel"));

		minWait = 4;
		maxWait = 6;
	}

	public override void OnStart ()
	{ }

	public override void OnUpdate ()
	{
	}

	public override void OnTakingDamage (TankInfo source)
	{ }

	public override void OnDeath (TankInfo source)
	{
		source.playerInfo.stats.barrels++;
		GameServer.Instance.SendStatsUpdate (source.playerInfo.orderNumber, source.playerInfo.stats);
		source.ShowStatsRecap ();
	}

	public override void OnRespawn (bool firstTime)
	{
		// TODO: make alpha scale from 0 to 1 here

	}

}
