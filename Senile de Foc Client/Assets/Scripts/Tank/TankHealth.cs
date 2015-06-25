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
		tankInfo = GetComponentInParent <TankInfo> ();
		spawnParticles = Utils.childWithName (tankInfo.transform, "Spawn Particles").GetComponent <ParticleSystem> ();;

		respawnTime = Constants.TANK_RESPAWN_TIME;

		hitters = new Dictionary<int, float> ();

		damaged = new ThresholdParticle[3];
		damaged [0] = new ThresholdParticle (75, Utils.childWithName (tankInfo.transform, "Lightly Damaged"), 	170, 190);// flame just in the back
		damaged [1] = new ThresholdParticle (50, Utils.childWithName (tankInfo.transform, "Medium Damaged"),	130, 260); // smoke goes in the back
		damaged [2] = new ThresholdParticle (25, Utils.childWithName (tankInfo.transform, "Heavily Damaged"),	60, 300); // leaks and sparks go anywhere but the front

		// Damaged particles
		minWait = 3;
		maxWait = 6;
	}

	override public void OnStart () 
	{ }

	override public void OnUpdate () 
	{ }
	
	public override void OnTakingDamage (float damage, TankInfo source)
	{
		// Hitters list is only kept in the instance on the machine that controls it
		if (tankInfo.isMine) {
			if (!hitters.ContainsKey (source.playerInfo.orderNumber))
				hitters [source.playerInfo.orderNumber] = 0;
			hitters [source.playerInfo.orderNumber] += damage;
		}
	}
	
	public override void OnDeath (TankInfo source)
	{
		// The update is announced by only one tank on one machine,
		// the tank that died on the machine that controls it
		if (tankInfo.isMine) {
			
			var thisOrderNr = tankInfo.playerInfo.orderNumber;

			var hittersList = hitters.ToList ();
			// Sorting by damage done (descending)
			hittersList.Sort ((a, b) => -a.Value.CompareTo (b.Value));

			hitters.Clear ();

			// Death always counts
			tankInfo.playerInfo.stats.deaths++;
			GameServer.Instance.SendStatsUpdate (thisOrderNr);

			var killer = hittersList [0].Key;
			// If a player has damaged himself more than the enemies,
			// the enemy that has damaged him the most takes the kill
			if (killer == thisOrderNr && hittersList.Count > 1)
				killer = hittersList [1].Key;
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

			camMovement.HandleDeath (); // camera doesn't go to the hidden, instead it can be controlled
			tankInfo.input.enabled = false; // disable movement and firing

			tankInfo.ShowStatsRecap (); // KD persistent show
			((IngameUIManager)IngameUIManager.Instance).state = IngameUIManager.State.dead; // show the dark overlay and respawn frame
			respawnCountdown.StartIt (respawnTime); // start the respawn timer on the respawn frame
			HittersDisplay.Instance.PopulateList (hittersList); // show the hitters and the damage percentage done
		}

		// Start the respawn timer on the scoreboard for this tank
		Scoreboard.Instance.StartCountdownFor (tankInfo.playerInfo.orderNumber, tankInfo.health.respawnTime); 
	}
	
	public override void OnRespawn (bool firstSpawn)
	{	
		spawnParticles.Play ();
		tankInfo.sounds.respawn.Play ();

		if (tankInfo.isMine && !firstSpawn) {
			tankInfo.input.enabled = true;
			camMovement.HandleRespawn ();

			// On the first spawn, positions are not randomly taken
			// they are given out in using the order number
			var point = GameWorld.RandomSpawnPoint ();
			// Keep the height, just move on up-down left-right
			transform.position = new Vector3 (point.position.x, point.position.y, transform.position.z);
			tankInfo.transform.rotation = point.rotation;
			transform.localRotation = point.rotation;

			((IngameUIManager)IngameUIManager.Instance).state = IngameUIManager.State.alive;
		}
	}
}
