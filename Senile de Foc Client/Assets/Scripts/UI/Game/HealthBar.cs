using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class HealthBar : Containable <HealthBar>
{
	RectTransform barFill;
	float fillLength;

	public PlayerStats player;
	public bool followPlayer = true;
	public Vector3 margin;

	void Awake ()
	{
		if (followPlayer)
			moveToContainer ("Health Bars");

		barFill = Utils.childWithName (transform, "Health Bar Fill").GetComponent <RectTransform> ();
		fillLength = barFill.sizeDelta.x;
	}

	void Update ()
	{
		if (followPlayer && player != null) {
			var pos = (Vector2)Camera.main.WorldToScreenPoint (player.transform.position + margin);
//			pos.y = Mathf.Clamp (pos.y, float.NegativeInfinity, Camera.main.WorldToScreenPoint (new Vector2 (0f, GameWorld.maxTop)).y); TODO
			transform.position = pos;
		}
	}

	public void Display (float amount, float maxAmount)
	{
		
		var pos = barFill.localPosition;
		var coeff = 1f - amount / maxAmount;
		pos.x = -coeff * fillLength;
		barFill.localPosition = pos;
	}
}
