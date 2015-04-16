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
	static Transform[] spawnTransforms;

	void Awake ()
	{
		// This isin't used anymore because the terrain is no longer a rectangle
		// it is a diamond cut off by a wall instead
//		BoundariesByTerrain ();
//		EdgesByBoundaries ();

		BoundariesByEdges ();

		// Getting the spawn positions
		var spawnPointsContainer = GameObject.Find ("Spawn Positions").transform;
		int n = spawnPointsContainer.childCount;
		spawnTransforms = new Transform[n];
		for (int i = 0; i < n; i++)
			spawnTransforms [i] = spawnPointsContainer.GetChild (i);
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
		foreach (var point in spawnTransforms) {
			var around = Physics2D.OverlapCircleAll (point.position, spawnClearRadius);
			if (Array.TrueForAll (around, coll => coll.tag != "Player"))
				available.Add (point);
		}

		return Utils.randomFrom (available.ToArray());
	}

//	public Transform terrainContainer;
//	void BoundariesByTerrain ()
//	{
//		float 
//			top = float.NegativeInfinity, 
//			bot = float.PositiveInfinity, 
//			left = float.PositiveInfinity, 
//			right = float.NegativeInfinity;
//		
//		// Looking for the max values according to the terrain
//		foreach (Transform terrain in terrainContainer) {
//			// I don't know why I have to multiply by 1.25...
//			// I suspect it's because the parent has scale 0.25...
//			top   = terrain.transform.position.y + terrain.transform.localScale.z * 1.25f;
//			bot   = terrain.transform.position.y - terrain.transform.localScale.z * 1.25f;
//			left  = terrain.transform.position.x - terrain.transform.localScale.x * 1.25f;
//			right = terrain.transform.position.x + terrain.transform.localScale.x * 1.25f;
//			
//			maxTop   = Mathf.Max (top,   maxTop);
//			maxBot   = Mathf.Min (bot,   maxBot);
//			maxLeft  = Mathf.Min (left,  maxLeft);
//			maxRight = Mathf.Max (right, maxRight);
//		}
//	}
//
//	void EdgesByBoundaries ()
//	{
//		// Setting up world edges according to the boundaries
//		topEdge  .localPosition = new Vector3 (0, maxTop, 0);
//		botEdge  .localPosition = new Vector3 (0, maxBot, 0);
//		leftEdge .localPosition = new Vector3 (maxLeft, 0, 0);
//		rightEdge.localPosition = new Vector3 (maxRight, 0, 0);
//		
//		topEdge .rotation = botEdge  .rotation = Quaternion.identity;
//		leftEdge.rotation = rightEdge.rotation = Quaternion.Euler (0, 0, 90f);
//		
//		topEdge.localScale = botEdge.localScale = new Vector3 (
//			Mathf.Abs (maxLeft - maxRight),	1f, 1f);
//		leftEdge.localScale = rightEdge.localScale = new Vector3 (
//			Mathf.Abs (maxTop - maxBot), 1f, 1f);
//	}
}
