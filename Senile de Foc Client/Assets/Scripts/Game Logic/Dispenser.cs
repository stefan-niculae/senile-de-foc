using UnityEngine;
using System.Collections;

public class Dispenser : Singleton<Dispenser> 
{
	public GameObject healthBarPrefab;
	static GameObject[] healthBars;

	void Awake ()
	{
		healthBars = new GameObject [Constants.MAX_PLAYERS - 1]; // the controlled plyer already has one
		for (int i = 0; i < healthBars.Length; i++)
			healthBars [i] = Instantiate (healthBarPrefab) as GameObject;
	}

	static int currentHealthBar;
	public static HealthBar TakeHealthBar (PlayerStats player)
	{
		var healthBar = healthBars [currentHealthBar].GetComponent <HealthBar> ();
		healthBar.player = player;
//		player.health.bar = healthBar; // we can't have circular dependenceis so we make the health script search for this bar when it awakens
		currentHealthBar++;
		return healthBar;
	}

	// TODO dispense starting spawn positions
}
