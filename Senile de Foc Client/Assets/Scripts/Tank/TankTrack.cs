using UnityEngine;
using System.Collections;

public class TankTrack : MonoBehaviour 
{
	public void MoveTo (Vector3 pos, Quaternion rot)
	{
		transform.position = pos;
		transform.rotation = rot;

		StartCoroutine (WaitAndDisappear ());
	}

	IEnumerator WaitAndDisappear ()
	{
		yield return new WaitForSeconds (TankTracks.timeToLive);
		transform.position = TankTracks.hidden;
	}
}
