using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class GameWorld : MonoBehaviour 
{
	public Transform
		topEdge,
		botEdge,
		leftEdge,
		rightEdge;

	public static float	
		maxTop,
		maxBot,
		maxLeft,
		maxRight;

	static float spawnClearRadius = 5f;
	public static Transform[] spawnPoints;

	void Awake ()
	{
		BoundariesByEdges ();

		// Getting the spawn points
		var spawnPointsContainer = GameObject.Find ("Spawn Positions").transform;
		spawnPoints = new Transform [4];
		spawnPoints [0] = Utils.childWithName (spawnPointsContainer, "Left Down");
		spawnPoints [1] = Utils.childWithName (spawnPointsContainer, "Left Up");
		spawnPoints [2] = Utils.childWithName (spawnPointsContainer, "Right Down");
		spawnPoints [3] = Utils.childWithName (spawnPointsContainer, "Right Up");
	}

	public void BoundariesByEdges ()
	{
		// The edges are manually set so we just take the the boundaries from the coordinates
		maxTop	 = topEdge.position.y;
		maxBot 	 = botEdge.position.y;
		maxLeft  = leftEdge.position.x;
		maxRight = rightEdge.position.x;
	}

	public static Transform RandomSpawnPoint ()
	{
		List<Transform> available = new List<Transform> ();
		foreach (var point in spawnPoints) {
			var around = Physics2D.OverlapCircleAll (point.position, spawnClearRadius);
			if (Array.TrueForAll (around, coll => coll.tag != "Player"))
				available.Add (point);
		}

		return Utils.randomFrom (available.ToArray());
	}
}
