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
	}

	void Start ()
	{
		amount = 100;
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
		foreach (var hitter in hitters)
			if (hitter != source)
				hitter.assists++;

		SpawnExplosion ();
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
}
