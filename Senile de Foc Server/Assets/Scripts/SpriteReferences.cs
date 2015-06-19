using UnityEngine;
using System.Collections;

public class SpriteReferences : MonoBehaviour 
{
	public Sprite positionMarker;
	public Sprite pointerMarker;

	public static SpriteReferences instance;
	void Awake ()
	{
		instance = this;
	}
}
