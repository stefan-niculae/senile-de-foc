using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	TankMovement movement;
	TankWeapon weapon;
	TankBarrel barrel;

	static int counter = 0;

	void Awake ()
	{
		// Simulating a singleton
		counter++;
		if (counter > 1)
			Debug.LogError ("More than one player input active!");

		movement = GetComponentInChildren <TankMovement> ();
		weapon = GetComponentInChildren <TankWeapon> ();
		barrel = GetComponentInChildren <TankBarrel> ();
	}
	
	void Update ()
	{
		// Weapon handling
		if (Input.GetButton ("Fire1"))
			weapon.Fire ();

		// Movement handling
		var horiz 	= Input.GetAxis ("Horizontal");
		var vert 	= Input.GetAxis ("Vertical");
		movement.Move (horiz, vert, true);

		// Barrel orientation handling
		var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		barrel.Rotate (Utils.LookAt2D (barrel.transform, mousePos));
	}
}
