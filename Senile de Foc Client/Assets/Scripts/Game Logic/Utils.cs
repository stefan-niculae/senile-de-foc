using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{
	void DistributionTest ()
	{
		int[] p = new int[] { 50, 30, 20 };
		int tests = 1000;

		int n = p.Length;
		GameObject[] o = new GameObject[n];
		for (int i = 0; i < n; i++)
			o [i] = new GameObject ();

		int[] c = new int[n];
		for (int i = 0; i < tests; i++)
			c [Array.IndexOf (o, randomFrom (o, p))]++;

		for (int i = 0; i < n; i++)
			if (tests/100 != 0)
				c [i] /= (tests / 100);

		string r = "theo\treal\n";
		for (int i = 0; i < n; i++)
			r += p [i] + "\t" + c [i] + "\n";
		Debug.LogFormat (r);
	}

	public static void ComputeBoundaries (Vector2 extents, ref float maxTop, ref float maxBot, ref float maxLeft, ref float maxRight)
	{
		maxTop   = GameWorld.maxTop   - extents.y;
		maxBot   = GameWorld.maxBot   + extents.y;
		maxLeft  = GameWorld.maxLeft  + extents.x;
		maxRight = GameWorld.maxRight - extents.x;
	}

	public static Vector2 ForwardDirection (Transform transf)
	{
		var angle = (transf.eulerAngles.magnitude + 90f) * Mathf.Deg2Rad;
		return new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
	}

	/**
	 * Returns the Z axis rotation when TRANSF is looking at TARGET
	 */
	public static Quaternion LookAt2D (Transform transf, Vector2 target)
	{
		// I dont know what's the math behind this
		var dir = target - (Vector2)transf.position;
		var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		var targetRot = Quaternion.AngleAxis (angle, Vector3.forward).eulerAngles.z - 90f;
		if (targetRot < 0)
			targetRot = 360f + targetRot;

		return Quaternion.Euler (new Vector3 (0, 0, targetRot));
	}

	public static Quaternion random2DRotation (float min = float.NaN, float max = float.NaN)
	{


		if (float.IsNaN (min) != float.IsNaN (max))
			Debug.LogError ("One is NaN, one is not");

		Vector3 euler = Vector3.zero;

		if (float.IsNaN (min)) {
			min = 0;
			max = 360;
		}
		euler.z = UnityEngine.Random.Range (min, max);
		return Quaternion.Euler (euler);
	}

	public static Quaternion rotate2D (	Quaternion from, float to)
	{
		var euler = from.eulerAngles;
		euler.z = to;
		return Quaternion.Euler (euler);
	}

	public static T randomFrom<T> (T[] array, int[] probabilities = null)
	{
		if (array == null || array.Length == 0)
			return default (T);

		// Equal probability
		if (probabilities == null)
			return array [UnityEngine.Random.Range (0, array.Length)];

		// Input validation
		int n = array.Length;

		if (n != probabilities.Length)
			Debug.LogErrorFormat ("Number of items ({0}) doesn't match number of probabilities ({1})", n, probabilities.Length);
		
		int sum = 0;
		foreach (int prob in probabilities)
			sum += prob;
		if (sum != 100)
			Debug.LogError ("Probabilities sum (" + sum + ") is not 100%");



		// Remembering the original indexes
		KeyValuePair<int, int>[] origIndex = new KeyValuePair<int, int>[n];
		for (int i = 0; i < n; i++) 
			origIndex[i] = new KeyValuePair<int, int> (probabilities[i], i);

		// Sorting by probability
		Array.Sort (origIndex, delegate (KeyValuePair<int, int> pair1, KeyValuePair<int, int> pair2) {
			return pair1.Key.CompareTo (pair1.Key);
		});

		// Rolling a random number
		float roll = UnityEngine.Random.Range (0, 101);

		int index = origIndex [n - 1].Value;
		int soFar = 0;
		// Array is sorted
		for (int i = 0; i < n; i++){
			soFar += origIndex [i].Key;
			if (roll < soFar) {
				index = origIndex [i].Value;
				break;
			}
		}

		return array [index];
	}
		
	public static bool CustomBetween (float value, float a, float b)
	{
		return 
			(
				// NaN cancels the script
				!float.IsNaN (value) &&
		    
		    	(
			     // a <= b or b <= a
				 (a <= value && value <= b) ||
				 (b <= value && value <= a)
				)
			);
	}

	public static Transform childWithName (Transform parent, string searchedName)
	{ 
		// Includes children of children and so on
		var children = parent.GetComponentsInChildren<Transform> ();

		foreach (var child in children)
			if (child.name == searchedName)
				return child;

		return null;
	}

	/**
	 * Returns the newly created game object
	 */ 
	public static GameObject replaceGO (GameObject oldObj, GameObject newObjPrefab)
	{
		var objParent = oldObj.transform.parent;
		var pos = oldObj.transform.position;
		var rot = oldObj.transform.rotation;
		Destroy (oldObj);

		GameObject created = Instantiate (newObjPrefab,
		                                  pos,
		                                  rot)
			as GameObject;
		created.transform.parent = objParent;
		return created;
	}

	/**
	 * Deletes every child of this transform
	 */
	public static void ClearContainer (Transform container)
	{
		var toDel = container.GetComponentsInChildren <Transform> ();
		
		// We do it this way because foreach child in container is buggy
		for (int i = 0; i < toDel.Length; i++)
			if (toDel[i] != null && toDel[i] != container && toDel[i].tag != "Polygon")
				DestroyImmediate (toDel[i].gameObject, false);
	}


	public static void NetworkDestroy (MonoBehaviour caller, GameObject obj, float time)
	{
		caller.StartCoroutine (Utils.WaitAndNetworkDestroy (obj, time));
	}

	static IEnumerator WaitAndNetworkDestroy (GameObject obj, float time)
	{
		yield return new WaitForSeconds (time);
		if (obj != null)
			Network.Destroy (obj);
	}

	public static string FloatToTime (float toConvert)
	{
		return string.Format("{0:#0}:{1:00}", 
			Mathf.FloorToInt (toConvert  / 60),	// minutes
			Mathf.FloorToInt (toConvert) % 60);	// seconds
	}
}
