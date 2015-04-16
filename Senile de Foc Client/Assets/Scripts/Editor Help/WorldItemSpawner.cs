using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class WorldItemSpawner : MonoBehaviour 
{
	public GameWorld world;


	public GameObject borderPrefab;
	public Transform borderContainer;
	public float borderInterval;
	public int borderNumber;

	public void SpawnBorder ()
	{
		// First delete the old border
		Delete (borderContainer, new string[] { "Grey Barrel Side" });

		// Get the boundaries
		world.BoundariesByEdges ();

		// For each edge
		for (int k = 1; k <= 4; k++) {
			for (int i = 0; i < borderNumber; i++) {

				Vector3 pos = transform.position;

				switch (k) {
				
				// Top border
				case 1: 
					pos.y = GameWorld.maxTop;
					pos.x = GameWorld.maxLeft + borderInterval * i;
					break;

				// Bottom
				case 2: 
					pos.y = GameWorld.maxBot;
					pos.x = GameWorld.maxLeft + borderInterval * i;
					break;
				
				// Left
				case 3:
					pos.x = GameWorld.maxLeft;
					pos.y = GameWorld.maxBot + borderInterval * i;
					break;
				
				// Right
				case 4: 
					pos.x = GameWorld.maxRight;
					pos.y = GameWorld.maxBot + borderInterval * i;
					break;
				
				default: 
					break;
				}


				GameObject spawnedObj = Instantiate (
					borderPrefab,
					pos,
					Utils.random2DRotation ()
				) as GameObject;

				spawnedObj.transform.parent = borderContainer;
			}
		}
	}

	void Delete (Transform container, string[] origNames)
	{
		Transform[] transforms = container.GetComponentsInChildren <Transform> (true);
		int children = transforms.Length;
		
		for (int i = 0; i < children; i++) {
			if (transforms[i] == null)
				continue;
			
			GameObject go = transforms[i].gameObject;
			foreach (var origName in origNames)
			if (go.name == origName + "(Clone)") {
				DestroyImmediate(go.gameObject, false);
				break;
			}
		}
	}
	
	public GameObject[] treePrefabs;
	public int[] treePrefabProbabilities;
	public Transform treeContainer;
	public Transform treePolygon;
	public float treeDeformationNiu;
	public int treeNumber;

	public void SpawnTrees ()
	{
		// Delete the old trees and get the boundaries
		Delete (treeContainer, new string[] { "treeSmall", "treeLarge" });
		SpawnObstacles (treePrefabs, treePrefabProbabilities, treeContainer, treePolygon, treeNumber, treeDeformationNiu, true);
	}

	public GameObject[] zone1BarrelPrefabs;
	public int[] zone1BarrelPrefabProbabilities;
	public Transform zone1BarrelContainer;
	public Transform zone1BarrelPolygon;
	public int zone1BarrelNumber;

	public void SpawnZone1Barrels ()
	{
		Delete (zone1BarrelContainer, new string[] { "Black Barrel", "Grey Barrel" });
		SpawnObstacles (zone1BarrelPrefabs, zone1BarrelPrefabProbabilities, zone1BarrelContainer, zone1BarrelPolygon, zone1BarrelNumber, 0, true);
	}

	public GameObject[] zone2BarrelPrefabs;
	public int[] zone2BarrelPrefabProbabilities;
	public Transform zone2BarrelContainer;
	public Transform zone2BarrelPolygon;
	public int zone2BarrelNumber;
	
	public void SpawnZone2Barrels ()
	{
		Delete (zone2BarrelContainer, new string[] { "Red Barrel", "Green Barrel" });
		SpawnObstacles (zone2BarrelPrefabs, zone2BarrelPrefabProbabilities, zone2BarrelContainer, zone2BarrelPolygon, zone2BarrelNumber, 0, true);
	}


	void SpawnObstacles (GameObject[] prefabs, int[] prefabProbabilities, Transform container, Transform polygon, int number, float deformation, bool randomlyRotate)
	{
		var pointTransforms = polygon.GetComponentsInChildren <Transform> (true).ToList ();
		pointTransforms.Remove (polygon); // the container itself is not a point
		var points = pointTransforms.Select (pointTransform => (Vector2)pointTransform.position).ToList (); // we only need the position

		// Should have initialized the min, max with the first value, not zero
		float maxLeft = 0, maxRight = 0, maxTop = 0, maxBot = 0;
		points.ForEach (point => 
			{
				maxLeft = Mathf.Min (point.x, maxLeft);
				maxRight = Mathf.Max (point.x, maxRight);
				maxTop = Mathf.Max (point.y, maxTop);
				maxBot = Mathf.Min (point.y, maxBot);
			}
		);

		int spawned = 0;
		int epoch = 0;
		while (!(spawned == number || epoch == Constants.MAX_EPOCH)) {
			Vector3 pos;
			pos.z = transform.position.z;

			pos.x = UnityEngine.Random.Range (maxLeft, maxRight);
			pos.y = Random.Range (maxTop, maxBot);

			if (isInside (pos, points)) {
				spawned++;

				GameObject spawnedObj = Instantiate (
					Utils.randomFrom (prefabs, prefabProbabilities),
					pos,
					randomlyRotate ? Utils.random2DRotation () : Quaternion.identity
				) as GameObject;

				spawnedObj.transform.parent = container;

				Vector3 scale = spawnedObj.transform.localScale;
				scale.x = 1 + Random.Range (-deformation, deformation);
				scale.y = 1 + Random.Range (-deformation,deformation);
				spawnedObj.transform.localScale = scale;
			}

			epoch++;
		}
	}


	bool isInside (Vector2 point, List <Vector2> polygon)
	{
		if (polygon.Count < 3)
			Debug.LogError ("Argument is not a polygon, it has less than three points!");

		int firstSide = side (polygon [0], polygon [1], point);
		for (int i = 2; i < polygon.Count; i++)
			if (side (polygon [i - 1], polygon [i], point) != firstSide)
				return false;

		return true;
	}
	
	/**
	 * on the same line => 0
	 * C is to the left of AB => +1
	 * C is to the right of AB => -1
	 */
	static int side (Vector2 A, Vector2 B, Vector2 C)
	{
		float val = (B.y - A.y) * (C.x - B.x) -
					(B.x - A.x) * (C.y - B.y);

		return Math.Sign (val);
	}
}
