using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour 
{
	public float damageAbsorbtion;
	public GameObject explosionPrefab;

	Transform bar;
	float fullLength;
	RectTransform UIBarFill;
	float UIFillLength;

	PlayerStats stats;

	List <PlayerStats> hitters;
	Vector3 hidden;
	CameraMovement camMovement;
	Countdown respawnCountdown;
	ParticleSystem spawnParticles;

	float _amount;
	float amount
	{
		get
		{
			return _amount;
		}

		set 
		{
			_amount = Mathf.Clamp (value, 0, 100);

			// Updating the on screen GUI bar for the player
			if (UIBarFill != null) {
				var pos = UIBarFill.localPosition;
				var coeff = 1f - _amount / 100f;
				pos.x = -coeff * UIFillLength;
				UIBarFill.localPosition = pos;
			}

			// And the above head thing bar for everyone else
			else {
				var scale = bar.localScale;
				scale.x = _amount / 100f * fullLength;
				bar.localScale = scale;
			}
		}
	}

	int 
		lightlyDamaged = 75,
		mediumDamaged = 50,
		heavilyDamaged = 25;

	ParticleSystem[] 
		lightlyDamagedParticles,
		mediumDamagedParticles,
		heavilyDamagedParticles;
		

	void Awake ()
	{
		hitters = new List <PlayerStats> ();
		hidden = Constants.HIDDEN;
		hidden.z = transform.position.z;

		stats = GetComponentInParent <PlayerStats> ();
		spawnParticles = Utils.childWithName (stats.transform, "Spawn Particles").GetComponent <ParticleSystem> ();;

		if (stats.controlledPlayer) {
			camMovement = Camera.main.GetComponent <CameraMovement> ();
			respawnCountdown = GameObject.Find ("Respawn Countdown").GetComponent <Countdown> ();
		}

		lightlyDamagedParticles = GetParticles ("Lightly Damaged");
		mediumDamagedParticles = GetParticles ("Medium Damaged");
		heavilyDamagedParticles = GetParticles ("Heavily Damaged");
		
		bar = Utils.childWithName (stats.transform, "Foreground");
		if (stats.controlledPlayer) {
			UIBarFill = GameObject.Find ("Health Bar Fill").GetComponent <RectTransform> ();
			UIFillLength = UIBarFill.sizeDelta.x;

			bar.parent.gameObject.SetActive (false);
			bar = null;
		}
		else 
			fullLength = bar.transform.localScale.x;


		amount = 100;
	}

	ParticleSystem[] GetParticles (string parentName)
	{
		int n;
		var parent = Utils.childWithName (stats.transform, parentName);
		n = parent.childCount;

		ParticleSystem[] particles = new ParticleSystem[n];
		for (int i = 0; i < n; i++)
			particles [i] = parent.GetChild (i).GetComponent <ParticleSystem> ();

		return particles;
	}

	void Start ()
	{
		StartCoroutine (Respawn (0, true));
	}
	
	void Update ()
	{
		// Tank damaged particles
		if (amount <= heavilyDamaged)
			PlayRandom (heavilyDamagedParticles, 170, 190); // flame just in the back
		else if (amount <= mediumDamaged)
			PlayRandom (mediumDamagedParticles, 130, 260); // smoke goes in the back // TODO: make these cooloer...
		else if (amount <= lightlyDamaged)
			PlayRandom (lightlyDamagedParticles, 60, 300); // leaks and sparks go anywhere but the front
	}

	float lastPlay;
	float toWait;
	void PlayRandom (ParticleSystem[] particles, float minRot = float.NaN, float maxRot = float.NaN)
	{
		// If enough time has passed since the last particle play
		if (Time.time - lastPlay >= toWait && particles != null && particles.Length != 0) {

			lastPlay = Time.time;
			toWait = Random.Range (3f, 6f);

			var parent = particles [0].transform.parent;
			parent.rotation = Utils.random2DRotation (minRot, maxRot);
			var particle = Utils.randomFrom (particles);
			if (particle != null)
					particle.Play ();

		}
	}


	void Reset ()
	{
		// Stats
		amount = 100;
		alreadyExploded = false;

		// Position
		var transf = GameWorld.RandomSpawnPoint ();
		var pos = transf.position;
		pos.z = transform.position.z;
		transform.position = pos;

		if (bar != null)
			bar.parent.gameObject.SetActive (true);

		// TODO: bug - this does not set for the controlled player
		transform.rotation = transf.rotation;
//		if (stats.controlledPlayer) Debug.Log (stats.username + " rotated to " + transform.rotation.eulerAngles);
	}

	bool alreadyExploded;
	public void TakeDamage (float damage, PlayerStats source)
	{
		// Apply the absorbtion
		damage *= 1f - damageAbsorbtion;

		// Apply the damage
		amount -= damage;

		// Add the hitter to the hitters list
		hitters.Add (source);

		// If it has killed the target
		if (amount <= 0 && !alreadyExploded) {
			alreadyExploded = true;
			Die (source);
		}
	}

	void Die (PlayerStats source)
	{
		// Increase this player's death count
		stats.deaths++;

		// And the other player's kill count
		if (source != stats) // but not on a suicide
			source.kills++;

		// And each hitter's (except the killer) assist count
		foreach (var hitter in hitters) {
			// TODO: shooty shoots abstract once, i kill shooty, i kill abstract
			// abstract gets a null reference on hitter
			if (hitter != source) 
				hitter.assists++;
		}
		// Clear the hitters list for the next death
		hitters.Clear ();

		SpawnExplosion ();

		if (camMovement != null)
			camMovement.HandleDeath ();

		// Disable the bar because it has a stay inside boundaries script
		bar.parent.gameObject.SetActive (false);

		// Teleport the body far away
		transform.position = hidden;

		// Start the countdown
		if (respawnCountdown != null)
			respawnCountdown.StartIt (stats.respawnTime);
		StartCoroutine (Respawn (stats.respawnTime));
	}

	void SpawnExplosion ()
	{
		alreadyExploded = true;

		GameObject explosion = Instantiate (
			explosionPrefab,
			transform.position,
			Quaternion.identity) as GameObject;
		explosion.transform.parent = GameObject.Find ("Explosions").transform;
	}

	public IEnumerator Respawn (float delay, bool ignoreCameraHandling = false)
	{
		yield return new WaitForSeconds (delay);

		spawnParticles.Play ();
		Reset ();

		if (camMovement != null && !ignoreCameraHandling)
			camMovement.HandleRespawn ();
	}

}
