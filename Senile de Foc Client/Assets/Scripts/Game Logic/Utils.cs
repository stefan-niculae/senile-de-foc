using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour 
{
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


		// Alternate solution (?)

		//		transf.LookAt (slightlyForward);
		//		var rot = transf.rotation.eulerAngles;
		//		rot.z = rot.x + rot.y;
		//		rot.y = rot.x = 0;
		//		rot.z = rot.z % 360;
		//		rot.z = rot.z > 180f ? rot.z - 180f : -rot.z;
		//		return Quaternion.Euler (rot);
	}
}
