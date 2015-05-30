using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TankHealth : Damagable
{
	TankInfo tankInfo;
	
	List <TankInfo> hitters;
	CameraMovement camMovement;
	Countdown respawnCountdown;
	ParticleSystem spawnParticles;

	override public void OnAwake ()
	{
		hitters = new List <TankInfo> ();
		
		tankInfo = GetComponentInParent <TankInfo> ();
		spawnParticles = Utils.childWithName (tankInfo.transform, "Spawn Particles").GetComponent <ParticleSystem> ();;
		
		if (tankInfo.isMine) {
			camMovement = Camera.main.GetComponent <CameraMovement> ();
			respawnCountdown = GameObject.Find ("Respawn Countdown").GetComponent <Countdown> ();
		}

		respawnTime = 2; // TODO set a reasonable respawn time

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
	
	public override void OnTakingDamage (TankInfo source)
	{
		hitters.Add (source);
	}
	
	public override void OnDeath (TankInfo source)
	{
		// Increase this player's death count
		tankInfo.deaths++;

		// The source will never be null because even if a barrel kills
		// still that barrel must have been shot by a player (and so on)

		// And the other player's kill count
		if (source != tankInfo) // but not on a suicide
			source.kills++;
		
		// And each hitter's (except the killer) assist count
		foreach (var hitter in hitters) {
			// TODO: shooty shoots abstract once, i kill shooty, i kill abstract
			// abstract gets a null reference on hitter
			if (hitter != source) {
//				hitter.assists++;
				//TODO
			}
		}
		// Clear the hitters list for the next death
		hitters.Clear ();

		if (camMovement != null)
			camMovement.HandleDeath ();

		// Start the countdown
		if (respawnCountdown != null)
			respawnCountdown.StartIt (respawnTime);
	}
	
	public override void OnRespawn (bool firstSpawn)
	{	
		if (camMovement != null && !firstSpawn)
			camMovement.HandleRespawn ();
		
		spawnParticles.Play ();

		// Position
		if (!firstSpawn) {
			var point = GameWorld.RandomSpawnPoint ();
			transform.position = point.position;
			transform.rotation = point.rotation;
		}
	}
}