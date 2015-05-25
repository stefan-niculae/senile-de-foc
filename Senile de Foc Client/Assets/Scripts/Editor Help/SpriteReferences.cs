using UnityEngine;
using System.Collections;

public class SpriteReferences : Singleton<SpriteReferences> 
{
	public Sprite[]
		bodies,
		barrels;

	public Sprite
		readyCheckmark;

	public Sprite
		invisible;
}
