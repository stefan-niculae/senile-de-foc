using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankTracksManager : MonoBehaviour 
{
	public GameObject trackPrefab;
	public Transform container;
	public int maxTracks = 250;
	public float treshold;

	public static Vector3 hidden = new Vector2 (1000f, 1000f);
	public static float timeToLive = 7f;
	LinkedList<TankTrack> tracks;

	void Start ()
	{
		hidden.z = transform.position.z;

		tracks = new LinkedList<TankTrack> ();
		for (int i = 0; i < maxTracks; i++) {
			GameObject spawned = Instantiate (
				trackPrefab,
				hidden,
				Quaternion.identity) as GameObject;

			spawned.transform.parent = container;
			tracks.AddLast (spawned.GetComponent<TankTrack> ());
		}
	}

	TankTrack toMove;
	Vector3 lastPos;

	public void Show (Vector3 pos, Quaternion rot)
	{
		// Do not spawn tracks too close to eachother
		if (Vector2.Distance (pos, lastPos) < treshold)
			return;
		lastPos = pos;

		pos.z = transform.position.z;

		// We take the last element in the list
		toMove = tracks.Last.Value;
		tracks.RemoveLast ();

		// Add it to the front
		tracks.AddFirst (toMove);

		toMove.MoveTo (pos, rot);
	}
}
