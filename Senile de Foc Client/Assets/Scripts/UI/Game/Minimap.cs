using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Minimap : Singleton<Minimap> 
{
	[HideInInspector] public Transform controlledPlayer;
	public RawImage map;
	public Image border;
	
	public float transp = .8f;

	public float UISize = .25f;
	public float UIBorder;
	Rect UIRect;
	
	void Start ()
	{
		float ratio = (float)Screen.width / Screen.height;
		
		// We do it in this order because the canvas is set to follow the height first
		UIRect.height = UISize;
		UIRect.width = UISize / ratio;
		
		UIRect.y = UIBorder;
		UIRect.x = 1f - UIRect.width - UIBorder / ratio;
	}

	bool isTransparent;
	void Update ()
	{
		if (controlledPlayer != null) {
			Vector2 mouseViewport = Camera.main.ScreenToViewportPoint (Input.mousePosition);
			Vector2 playerViewport = Camera.main.WorldToViewportPoint (controlledPlayer.position);
			if (UIRect.Contains (mouseViewport) || UIRect.Contains (playerViewport) || Scoreboard.Instance.isShown) {
				if (!isTransparent) {
					isTransparent = true;
					map.color = ApplyTransparency (map.color, transp);
					border.color = ApplyTransparency (border.color, transp);
				}
			} else {
				if (isTransparent) {
					isTransparent = false;
					map.color = ApplyTransparency (map.color, 1f / transp);
					border.color = ApplyTransparency (border.color, 1f / transp);
				}
			}
		}
	}

	Color ApplyTransparency (Color color, float percentage)
	{
		var tempCol = color;
		tempCol.a *= percentage;
		return tempCol;
	}
}
