using UnityEngine;
using System.Collections;

public class TankSounds : MonoBehaviour 
{
	[HideInInspector] public AudioSource
		tracks,
		engine,
		cannon,
		special,
		death,
		respawn;

	const float MAX_TRACKS_VOL = .2f;
	public float tracksVolume
	{
		get { return tracks.volume; }
		set { tracks.volume = value * MAX_TRACKS_VOL; }
	}

	void Awake ()
	{
		tracks 	= Utils.childWithName (transform, "Tracks Moving")	.GetComponent <AudioSource> ();
		engine 	= Utils.childWithName (transform, "Engine Start")	.GetComponent <AudioSource> ();
		cannon 	= Utils.childWithName (transform, "Cannon Fire")	.GetComponent <AudioSource> ();
		special = Utils.childWithName (transform, "Special Fire")	.GetComponent <AudioSource> ();
		death 	= Utils.childWithName (transform, "Death")			.GetComponent <AudioSource> ();
		respawn = Utils.childWithName (transform, "Respawn")		.GetComponent <AudioSource> ();
	}

}
