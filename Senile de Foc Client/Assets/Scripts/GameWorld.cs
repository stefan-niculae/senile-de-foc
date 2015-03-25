using UnityEngine;
using System.Collections;

public class GameWorld : MonoBehaviour 
{
	public Transform
		topEdge,
		botEdge,
		leftEdge,
		rightEdge;

	public Transform terrainContainer;

	public static float	
		maxTop,
		maxBot,
		maxLeft,
		maxRight;

	void Awake ()
	{
		float 
			top = float.NegativeInfinity, 
			bot = float.PositiveInfinity, 
			left = float.PositiveInfinity, 
			right = float.NegativeInfinity;

		// Looking for the max values
		foreach (Transform terrain in terrainContainer) {
			// I don't know why I have to multiply by 1.25...
			// I suspect it's because the parent has scale 0.25...
			top   = terrain.transform.position.y + terrain.transform.localScale.z * 1.25f;
			bot   = terrain.transform.position.y - terrain.transform.localScale.z * 1.25f;
			left  = terrain.transform.position.x - terrain.transform.localScale.x * 1.25f;
			right = terrain.transform.position.x + terrain.transform.localScale.x * 1.25f;

			maxTop   = Mathf.Max (top,   maxTop);
			maxBot   = Mathf.Min (bot,   maxBot);
			maxLeft  = Mathf.Min (left,  maxLeft);
			maxRight = Mathf.Max (right, maxRight);
		}

		// Setting up world edges
		topEdge  .localPosition = new Vector3 (0, maxTop, 0);
		botEdge  .localPosition = new Vector3 (0, maxBot, 0);
		leftEdge .localPosition = new Vector3 (maxLeft, 0, 0);
		rightEdge.localPosition = new Vector3 (maxRight, 0, 0);

		topEdge .rotation = botEdge  .rotation = Quaternion.identity;
		leftEdge.rotation = rightEdge.rotation = Quaternion.Euler (0, 0, 90f);

		topEdge.localScale = botEdge.localScale = new Vector3 (
			Mathf.Abs (maxLeft - maxRight),	1f, 1f);
		leftEdge.localScale = rightEdge.localScale = new Vector3 (
			Mathf.Abs (maxTop - maxBot), 1f, 1f);

	}

}
