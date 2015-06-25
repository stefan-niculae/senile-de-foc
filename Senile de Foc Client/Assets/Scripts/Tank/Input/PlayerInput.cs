using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour 
{
	TankMovement movement;
	TankBarrel barrel;
	Cannon cannon;
	Secondary secondary;

	void Awake ()
	{
		movement = GetComponentInChildren <TankMovement> ();
		cannon = GetComponentInChildren <Cannon> ();
		secondary = GetComponentInChildren <Secondary> ();
		barrel = GetComponentInChildren <TankBarrel> ();
	}
	
	void Update ()
	{
		// Weapon handling
		if (!IngameUIManager._pointerOverButton) {
			if (Input.GetButton ("Fire1"))
				cannon.Fire ();
			if (Input.GetButton ("Fire2"))
				secondary.Fire ();
		}

		// Movement handling
		var horiz 	= Input.GetAxis ("Horizontal");
		var vert 	= Input.GetAxis ("Vertical");
		movement.Move (horiz, vert);

		// Barrel orientation handling
		var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		barrel.Rotate (Utils.LookAt2D (barrel.transform, mousePos));
	}
}
