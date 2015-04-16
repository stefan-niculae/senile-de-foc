using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankHealth : MonoBehaviour 
{
	public Transform bar;
	public float fullLength;
	public GameObject explosionPrefab;
	public PlayerStats stats;

	List <PlayerStats> hitters;
	Vector3 hidden;
	CameraMovement camMovement = null;
	Countdown respawnCountdown = null;

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

			// Update the UI health bar
			var scale = bar.localScale;
			scale.x = _amount / 100f * fullLength;
			bar.localScale = scale;
		}
	}

	void Awake ()
	{
		hitters = new List <PlayerStats> ();
		hidden = Constants.HIDDEN;
		hidden.z = transform.position.z;

		if (stats.controlledPlayer) {
			camMovement = Camera.main.GetComponent <CameraMovement> ();
			respawnCountdown = GameObject.Find ("Respawn Countdown").GetComponent <Countdown> ();
		}
	}

	void Start ()
	{
		Reset ();
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

		bar.parent.gameObject.SetActive (true);

		// TODO: bug - this does not set for the controlled player
		transform.rotation = transf.rotation;
		if (stats.controlledPlayer) Debug.Log (name + " rotated to " + transform.rotation.eulerAngles);
	}

	bool alreadyExploded;
	public void TakeDamage (float damage, PlayerStats source)
	{
		// Apply the damage
		amount -= damage;

		// Add the hitter to the hitters list
		hitters.Add (source);

		// If it has killed the target
		if (amount == 0 && !alreadyExploded) {
			alreadyExploded = true;
			Die (source);
		}
	}

	void Die (PlayerStats source)
	{
		// Increase this player's death count
		stats.deaths++;

		// And the other player's kill count
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

	public IEnumerator Respawn (float delay)
	{
		yield return new WaitForSeconds (delay);

		Reset ();

		if (camMovement != null)
			camMovement.HandleRespawn ();
	}

}
