using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class EnvironmentItemSpawner : MonoBehaviour 
{
	[System.Serializable]
	public class PrefabProbability
	{
		public GameObject prefab;
		public int probability;
	}

	public PrefabProbability[] items;
	public Transform polygon;
	public Transform container;
	public int number;
	public bool randomlyRotate;
	public float deformation;

	float maxLeft, maxRight, maxTop, maxBot;

	public void SpawnItems ()
	{
		// Remove the previously spawned items
		Utils.ClearContainer (container);

		List<Vector2> points = ExtractPoints (polygon);
		FindBoundingBox (points);

		// The epoch failsafe is here not because the random could make the call not terminate
		// but because we place an extra condition - items may not override
		// and thus the algorithm may be asked to spawn more items than the available space
		int spawned = 0, epoch = 0;
		while (spawned < number && epoch < Constants.MAX_EPOCH) {
			var successfulySpawned = Spawn (points);
			if (successfulySpawned)
				spawned++;
			epoch++;
		}
	}

	List<Vector2> ExtractPoints (Transform polygon)
	{
		// First we extract the transforms
		var transforms = polygon.GetComponentsInChildren <Transform> (true).ToList ();
		// The container itself is not a corner of the polygon
		transforms.Remove (polygon);
		// Then we convert to Vector2's
		return transforms.Select (transf => (Vector2)transf.position).ToList ();
	}

	void FindBoundingBox (List<Vector2> points)
	{
		maxLeft = 	maxRight = 	points [0].x;
		maxTop = 	maxBot = 	points [0].y;
	
		points.ForEach (point => 
		    {
				maxLeft = 	Mathf.Min (point.x, maxLeft);
				maxRight =	Mathf.Max (point.x, maxRight);
				maxTop = 	Mathf.Max (point.y, maxTop);
				maxBot = 	Mathf.Min (point.y, maxBot);
			}
		);
	}

	bool Spawn (List<Vector2> polygon)
	{
		Vector3 pos = RandomInBoundingBox ();
		GetItemToSpawn ();
	
		// Check if the item is inside the polygon and it doesn't overlap something else
		if (isInside (pos, polygon) && !Overlaps (pos)) {

			// Spawn the item
			GameObject spawnedObj = Instantiate (
					toSpawn,
					pos,
					randomlyRotate ? Utils.random2DRotation () : Quaternion.identity
				) as GameObject;

			// Place it in the container
			spawnedObj.transform.parent = container;

			Deform (spawnedObj);

			// Generate a new item to spawn
			toSpawn = null;
			return true;
		}

		return false;
	}

	GameObject[] prefabs;
	int[] probabilities;
	bool converted;
	GameObject toSpawn;
	void GetItemToSpawn ()
	{
		// Converting array of GO + int to array of GO + array of int
		if (!converted) {
			int n = items.Length;
			prefabs = new GameObject[n];
			probabilities = new int[n];

			for (int i = 0; i < n; i++) {
				prefabs[i] = items[i].prefab;
				probabilities[i] = items[i].probability;
			}
			converted = true;
		}

		if (toSpawn == null) 
			toSpawn = Utils.randomFrom (prefabs, probabilities);

	}

	Vector3 RandomInBoundingBox ()
	{
		Vector3 point;
		point.z = transform.position.z;
		
		point.x = Random.Range (maxLeft, maxRight);
		point.y = Random.Range (maxBot, maxTop);

		return point;
	}

	bool Overlaps (Vector3 pos)
	{
		var radius = toSpawn.GetComponent <CircleCollider2D> ().radius;
		return Physics2D.OverlapCircleAll (pos, radius).Length != 0;
	}

	bool isInside (Vector2 point, List <Vector2> polygon)
	{
		if (polygon.Count < 3)
			Debug.LogError ("Argument is not a polygon, it has less than three points!");

		int n = polygon.Count;
		List<Vector2> poly = polygon;
		poly.Add (polygon [0]); // Polygon[n+1] = Polygon[0]

		int windingNumber = 0;

		// For each edge in the polygon
		for (int i = 0; i < n; i++) 

			if (poly [i].y <= point.y) {          
				if (poly [i+1].y  > point.y)      
					if (side (poly [i], poly [i+1], point) > 0)  // point left of  edge
						windingNumber++;            
			}
			else {                        
				if (poly [i+1].y  <= point.y)     
					if (side (poly [i], poly [i+1], point) < 0)  // point right of  edge
						windingNumber--;            
			}
			
		return windingNumber != 0;
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

	void Deform (GameObject item)
	{
		Vector3 scale = item.transform.localScale;
		scale.x = 1 + Random.Range (-deformation, deformation);
		scale.y = 1 + Random.Range (-deformation,deformation);
		item.transform.localScale = scale;
	}
}
