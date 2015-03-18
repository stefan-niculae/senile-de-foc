using UnityEngine;
using System.Collections;

public class TankWeapon : MonoBehaviour 
{
	public TankBarrel barrel;

	void Update ()
	{
		if (Input.GetButtonUp ("Fire1"))
			Fire ();
	}

	void Fire  ()
	{
		barrel.Bounce ();
	}
}
