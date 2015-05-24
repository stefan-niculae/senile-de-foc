using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour 
{
	Vector3 shown;
	public static bool isShown;

	void Awake ()
	{
		shown = transform.localPosition;
	}

	void Update ()
	{
		isShown = Input.GetKey (KeyCode.Tab);

		transform.localPosition = (isShown ? shown : Constants.HIDDEN);
	}
}
