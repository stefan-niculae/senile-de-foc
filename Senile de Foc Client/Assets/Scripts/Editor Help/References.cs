using UnityEngine;
using System.Collections;

public class References : Singleton<References> 
{
	public Sprite[]
		bodySprites,
		barrelSprites;

	public Sprite
		readyCheckmark;

	public Sprite
		invisibleSprite;

	public GameObject tankPrefab;
	public GameObject aboveInfoPrefab;
}
