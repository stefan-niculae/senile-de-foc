using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	TankMovement movement;
	Cannon cannon;
	Secondary secondary;
	TankBarrel barrel;

	static int counter = 0;

	void Awake ()
	{
		// Simulating a singleton
		counter++;
		if (counter > 1)
			Debug.LogError ("More than one player input active!");

		movement = GetComponentInChildren <TankMovement> ();
		cannon = GetComponentInChildren <Cannon> ();
		secondary = GetComponentInChildren <Secondary> ();
		barrel = GetComponentInChildren <TankBarrel> ();
	}
	
	void Update ()
	{
		// Weapon handling
		if (Input.GetButton ("Fire1"))
			cannon.Fire ();
		if (Input.GetButton ("Fire2")) { 
			secondary.Fire ();
		}

		// Movement handling
		var horiz 	= Input.GetAxis ("Horizontal");
		var vert 	= Input.GetAxis ("Vertical");
		movement.Move (horiz, vert, true);

		// Barrel orientation handling
		var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		barrel.Rotate (Utils.LookAt2D (barrel.transform, mousePos));
	}
}
