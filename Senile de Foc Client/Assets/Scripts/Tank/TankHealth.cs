using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class TankHealth : Damagable
{
	[HideInInspector] public TankInfo tankInfo;
	
	Dictionary <int, float> hitters;
	[HideInInspector] public CameraMovement camMovement;
	[HideInInspector] public Countdown respawnCountdown;
	ParticleSystem spawnParticles;

	override public void OnAwake ()
	{
		hitters = new Dictionary<int, float> ();
		
		tankInfo = GetComponentInParent <TankInfo> ();
		spawnParticles = Utils.childWithName (tankInfo.transform, "Spawn Particles").GetComponent <ParticleSystem> ();;

		respawnTime = Constants.TANK_RESPAWN_TIME;

		damaged = new ThresholdParticle[3];
		damaged [0] = new ThresholdParticle (75, Utils.childWithName (tankInfo.transform, "Lightly Damaged"), 	170, 190);// flame just in the back
		damaged [1] = new ThresholdParticle (50, Utils.childWithName (tankInfo.transform, "Medium Damaged"),	130, 260); // smoke goes in the back
		damaged [2] = new ThresholdParticle (25, Utils.childWithName (tankInfo.transform, "Heavily Damaged"),	60, 300); // leaks and sparks go anywhere but the front

		// Damaged particles
		minWait = 3;
		maxWait = 6;
	}

	public void RegisterNetworkID ()
	{
		networkID = tankInfo.playerInfo.orderNumber;
		RegisterThis (networkID);
	}

	override public void OnStart () 
	{ }

	override public void OnUpdate () 
	{ }
	
	public override void OnTakingDamage (float damage, TankInfo source)
	{
		if (!hitters.ContainsKey (source.playerInfo.orderNumber))
			hitters [source.playerInfo.orderNumber] = 0;
		hitters [source.playerInfo.orderNumber] += damage;

		Debug.LogFormat ("{0} has done {1} dmg to {2}", source.playerInfo.name, hitters [source.playerInfo.orderNumber], tankInfo.playerInfo.name);
	}
	
	public override void OnDeath (TankInfo source)
	{
		var thisOrderNr = tankInfo.playerInfo.orderNumber;

		var hittersList = hitters.ToList ();
		// Sorting by damage done (descending)
		hittersList.Sort ((a, b) => -a.Value.CompareTo (b.Value));

		hitters.Clear ();

		// Death always counts
		tankInfo.playerInfo.stats.deaths++;
		GameServer.Instance.SendStatsUpdate (thisOrderNr);

		var killer = hittersList [0].Key;
		// Suicides don't count as kills
		if (killer != thisOrderNr) {
			GameServer.Instance.orderNrToTankInfo [killer].playerInfo.stats.kills++;
			GameServer.Instance.SendStatsUpdate (killer);
		}

		for (int i = 1; i < hittersList.Count; i++) {
			var assistant = hittersList [i].Key;
			// Hitting yourself doesn't grant you an assist
			if (assistant != thisOrderNr) {
				GameServer.Instance.orderNrToTankInfo [assistant].playerInfo.stats.assists++;
				GameServer.Instance.SendStatsUpdate (assistant);
			}
		}


		if (tankInfo.isMine) {
			tankInfo.ShowStatsRecap ();
			camMovement.HandleDeath ();
			((IngameUIManager)IngameUIManager.Instance).state = IngameUIManager.State.dead;
			tankInfo.input.enabled = false;
			respawnCountdown.StartIt (respawnTime);
			HittersDisplay.Instance.PopulateList (hittersList);
		}
	}

	public override void OnZeroHealth ()
	{
		Scoreboard.Instance.StartCountdownFor (tankInfo.playerInfo.orderNumber, tankInfo.health.respawnTime);
	}
	
	public override void OnRespawn (bool firstSpawn)
	{	
		if (camMovement != null && !firstSpawn)
			camMovement.HandleRespawn ();

		if (tankInfo.isMine && !firstSpawn)
			tankInfo.input.enabled = true;
		
		spawnParticles.Play ();

		// On the first spawn, positions are not randomly taken
		// they are given out in using the order number
		if (!firstSpawn) {
			var point = GameWorld.RandomSpawnPoint ();
			// Keep the height, just move on up-down left-right
			transform.position = new Vector3 (point.position.x, point.position.y, transform.position.z);
			tankInfo.movement.rot = point.rotation.eulerAngles;
			transform.localRotation = point.rotation;

			((IngameUIManager)IngameUIManager.Instance).state = IngameUIManager.State.alive;
		}
	}
}
