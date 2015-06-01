using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TankHealth : Damagable
{
	TankInfo tankInfo;
	
	List <TankInfo> hitters;
	[HideInInspector] public CameraMovement camMovement;
	Countdown respawnCountdown;//unused
	ParticleSystem spawnParticles;

	override public void OnAwake ()
	{
		hitters = new List <TankInfo> ();
		
		tankInfo = GetComponentInParent <TankInfo> ();
		spawnParticles = Utils.childWithName (tankInfo.transform, "Spawn Particles").GetComponent <ParticleSystem> ();;

		respawnTime = 5; // TODO set a reasonable respawn time

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
		RegisterThis ();
	}

	override public void OnStart () 
	{ }

	override public void OnUpdate () 
	{ }
	
	public override void OnTakingDamage (TankInfo source)
	{
		hitters.Add (source);
	}
	
	public override void OnDeath (TankInfo source)
	{
		var haveStatsUpdated = new HashSet <TankInfo> ();

		// Increase this player's death count
		tankInfo.playerInfo.stats.deaths++;
		haveStatsUpdated.Add (tankInfo);

		// The source will never be null because even if a barrel kills
		// still that barrel must have been shot by a player (and so on)

		// And the other player's kill count
		if (source != tankInfo) {// but not on a suicide
			source.playerInfo.stats.kills++;
			haveStatsUpdated.Add (source);
		}
		
		// And each hitter's (except the killer) assist count
		foreach (var hitter in hitters) 
			// TODO: shooty shoots abstract once, i kill shooty, i kill abstract
			// abstract gets a null reference on hitter
			if (hitter != source) {
				hitter.playerInfo.stats.assists++;
				haveStatsUpdated.Add (hitter);
			}

		foreach (var tank in haveStatsUpdated) {
			GameServer.Instance.SendStatsUpdate (tank.playerInfo.orderNumber, tank.playerInfo.stats);
			tank.ShowStatsRecap ();
		}

		// Clear the hitters list for the next death
		hitters.Clear ();

		if (tankInfo.isMine) {
			camMovement.HandleDeath ();
			UIManager.Instance.DeadView ();
		}

		// Start the countdown
		if (respawnCountdown != null)
			respawnCountdown.StartIt (respawnTime);
	}
	
	public override void OnRespawn (bool firstSpawn)
	{	
		if (camMovement != null && !firstSpawn)
			camMovement.HandleRespawn ();
		
		spawnParticles.Play ();

		// On the first spawn, positions are not randomly taken
		// they are given out in using the order number
		if (!firstSpawn) {
			var point = GameWorld.RandomSpawnPoint ();
			// Keep the height, just move on up-down left-right
			transform.position = new Vector3 (point.position.x, point.position.y, transform.position.z);
			tankInfo.movement.rot = point.rotation.eulerAngles;
			transform.localRotation = point.rotation;

			UIManager.Instance.AliveView ();
		}
	}
}