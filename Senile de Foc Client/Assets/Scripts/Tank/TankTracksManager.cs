using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankTracksManager : Containable<TankTracksManager> 
{
	const int 	MAX_TRACKS 	= 100;
	const float THRESHOLD 	= .25f;

	TankTrack[] tracks;

	void Awake ()
	{
		moveToContainer ("Track Managers");
		transform.position = new Vector3 (0, 0, 1);
	}

	void Start ()
	{
		var hidden = Constants.HIDDEN;
		hidden.z = transform.position.z;
		TankTrack.hidden = hidden;

		tracks = new TankTrack [MAX_TRACKS];
		for (int i = 0; i < MAX_TRACKS; i++) {
			GameObject spawned = Instantiate (
				References.Instance.trackPrefab,
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
		if (Vector2.Distance (pos, lastPos) < THRESHOLD)
			return;
		lastPos = pos;

		pos.z = transform.position.z;

		tracks[index].MoveTo (pos, rot);
		// Circularly get to the next index
		index = (index + 1) % MAX_TRACKS;
	}
}
