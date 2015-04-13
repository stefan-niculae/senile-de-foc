using UnityEngine;
using System.Collections;

public class FiringAI : MonoBehaviour 
{
	TankWeapon weapon;
	public float rate;
	public bool audioEnabled;

	void Awake ()
	{
		weapon = GetComponentInChildren <TankWeapon> ();
	}

	void Start ()
	{
		InvokeRepeating ("FireWrapper", 0, rate);
	}

	// We use a wrapper because invoke repeating requires a string
	void FireWrapper ()
	{
		weapon.Fire (false);
	}

}
