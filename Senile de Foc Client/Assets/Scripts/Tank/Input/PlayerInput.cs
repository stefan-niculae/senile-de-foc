using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	TankMovement movement;
	TankWeapon weapon;

	static int counter = 0;

	void Awake ()
	{
		// Simulating a singleton
		counter++;
		if (counter > 1)
			Debug.LogError ("More than one player input active!");

		movement = GetComponentInChildren <TankMovement> ();
		weapon = GetComponentInChildren <TankWeapon> ();
	}

	float horiz, vert;
	void Update ()
	{
		// Weapon handling
		if (Input.GetButton ("Fire1"))
			weapon.Fire ();

		// Movement handling
		horiz 	= Input.GetAxis ("Horizontal");
		vert 	= Input.GetAxis ("Vertical");
		movement.Move (horiz, vert, true);
	}
}
