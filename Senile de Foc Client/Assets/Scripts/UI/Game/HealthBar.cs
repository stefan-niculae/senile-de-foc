using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class HealthBar : MonoBehaviour
{
	RectTransform barFill;
	float fillLength;

	void Awake ()
	{
		barFill = Utils.childWithName (transform, "Health Bar Fill").GetComponent <RectTransform> ();
		fillLength = barFill.sizeDelta.x;
	}

	public void Display (float amount, float maxAmount)
	{
		var pos = barFill.localPosition;
		var coeff = 1f - amount / maxAmount;
		pos.x = -coeff * fillLength;
		barFill.localPosition = pos;
	}
}
