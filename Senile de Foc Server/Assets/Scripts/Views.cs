using UnityEngine;
using System.Collections;

public class Views : MonoBehaviour 
{
	public Transform movable;
	bool menuVisible = true;

	public void ToggleMenu ()
	{
		menuVisible = !menuVisible;
		if (menuVisible)
			movable.localPosition = Vector3.zero;
		else
			movable.localPosition = 10000 * Vector2.one;
	}
}
