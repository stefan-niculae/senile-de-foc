using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankTracksManager : MonoBehaviour 
{
	public GameObject trackPrefab;
	public int maxTracks = 250;
	public float treshold;

	TankTrack[] tracks;

	void Start ()
	{
		var hidden = Constants.HIDDEN;
		hidden.z = transform.position.z;
		TankTrack.hidden = hidden;

		tracks = new TankTrack[maxTracks];
		for (int i = 0; i < maxTracks; i++) {
			GameObject spawned = Instantiate (
				trackPrefab,
				hidden,
				Quaternion.identity) as GameObject;

			tracks[i] = spawned.GetComponent <TankTrack> ();
		}
	}

	int index;
	Vector3 lastPos;
	public void Show (Vector3 pos, Quaternion rot)
	{
		// Do not spawn tracks too close to eachother
		if (Vector2.Distance (pos, lastPos) < treshold)
			return;
		lastPos = pos;

		pos.z = transform.position.z;

		tracks[index].MoveTo (pos, rot);
		// Circularly get to the next index
		index = (index + 1) % maxTracks;
	}
}
