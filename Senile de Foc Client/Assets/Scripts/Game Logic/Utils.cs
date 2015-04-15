using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Utils : MonoBehaviour 
{
	// TODO: make more tests on this function and also move them in a tests file!!!
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
		// TODO: FKIN GET THIS TO WORK X(
		// I dont know what's the math behind this
		var dir = target - (Vector2)transf.position;
		var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		var targetRot = Quaternion.AngleAxis (angle, Vector3.forward).eulerAngles.z - 90f;
		if (targetRot < 0)
			targetRot = 360f + targetRot;

		return Quaternion.Euler (new Vector3 (0, 0, targetRot));
	}

	public static Quaternion random2DRotation ()
	{
		Quaternion rot = UnityEngine.Random.rotation;
		Vector3 euler = rot.eulerAngles;
		euler.x = euler.y = 0;
		return Quaternion.Euler (euler);
	}

	// TODO: make this generic
	public static GameObject randomFrom (GameObject[] array, int[] probabilities = null)
	{
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
}
