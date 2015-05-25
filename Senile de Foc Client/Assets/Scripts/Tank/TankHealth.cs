using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TankHealth : Damagable
{
	PlayerStats stats;
	
	List <PlayerStats> hitters;
	CameraMovement camMovement;
	Countdown respawnCountdown;
	ParticleSystem spawnParticles;

	override public void OnAwake ()
	{
		hitters = new List <PlayerStats> ();
		
		stats = GetComponentInParent <PlayerStats> ();
		spawnParticles = Utils.childWithName (stats.transform, "Spawn Particles").GetComponent <ParticleSystem> ();;
		
		if (stats.controlledPlayer) {
			camMovement = Camera.main.GetComponent <CameraMovement> ();
			respawnCountdown = GameObject.Find ("Respawn Countdown").GetComponent <Countdown> ();
		}
		
		maxHp = 100;
		respawnTime = 2;

		damaged = new ThresholdParticle[3];
		damaged [0] = new ThresholdParticle (75, Utils.childWithName (stats.transform, "Lightly Damaged"), 	170, 190);// flame just in the back
		damaged [1] = new ThresholdParticle (50, Utils.childWithName (stats.transform, "Medium Damaged"),	130, 260); // smoke goes in the back // TODO: make these cooloer...
		damaged [2] = new ThresholdParticle (25, Utils.childWithName (stats.transform, "Heavily Damaged"),	60, 300); // leaks and sparks go anywhere but the front

		// Damaged particles
		minWait = 3;
		maxWait = 6;
	}
	
	override public void OnStart () 
	{
	}

	override public void OnUpdate () 
	{
	}
	
	public override void OnTakingDamage (PlayerStats source)
	{
		hitters.Add (source);
	}
	
	public override void OnDeath (PlayerStats source)
	{
		// Increase this player's death count
		stats.deaths++;

		// The source will never be null because even if a barrel kills
		// still that barrel must have been shot by a player (and so on)

		// And the other player's kill count
		if (source != stats) // but not on a suicide
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
		var transf = GameWorld.RandomSpawnPoint ();
		var pos = transf.position; 
		pos.z = transform.position.z;
		if (stats.move || !firstSpawn)
			transform.position = pos;
		
		// TODO: bug - this does not set for the controlled player
		transform.rotation = transf.rotation;
		//		if (stats.controlledPlayer) Debug.Log (stats.username + " rotated to " + transform.rotation.eulerAngles);
	}
}