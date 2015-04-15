using UnityEngine;
using System.Collections;

public class TankTrack : MonoBehaviour 
{
	public static float timeToLive = 7f;
	public static Vector3 hidden;
	float mostRecentMove;

	public void MoveTo (Vector3 pos, Quaternion rot)
	{
		mostRecentMove = Time.time;

		transform.position = pos;
		transform.rotation = rot;

		StartCoroutine (WaitAndDisappear ());
	}
	
	IEnumerator WaitAndDisappear ()
	{
		yield return new WaitForSeconds (timeToLive);

		// While waiting to dissapear this track may have been moved again
		// so we check if /time_to_live/ seconds have passed since the most recent move
		// and only then we hide the track
		if (Mathf.Abs ((Time.time - mostRecentMove) - timeToLive) <= Constants.EPSION)
			transform.position = hidden;
	}
}
