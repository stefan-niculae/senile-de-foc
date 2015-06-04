using UnityEngine;
using System.Collections;

public class References : Singleton<References> 
{
	public Sprite[]
		bodySprites,
		barrelSprites;

	public Sprite readyCheckmark;

	public Sprite invisibleSprite;

	public GameObject tankPrefab;
	public GameObject aboveInfoPrefab;

	public GameObject[]
		projectilePrefabs,
		specialPrefabs;

	public Sprite[]
		primarySprites,
		secondarySprites;

	public GameObject trackManagerPrefab;
	public GameObject trackPrefab;

	public Color 
		aliveColor,
		deadColor;

	public AudioClip[]
		bulletSounds,
		missileSounds;
	public AudioClip[]
		secondarySounds;
}
