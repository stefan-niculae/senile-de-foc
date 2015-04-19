using UnityEngine;
using System.Collections;

public class FiringAI : MonoBehaviour 
{
	Cannon weapon;
	public float rate;
	public bool audioEnabled;

	void Awake ()
	{
		weapon = GetComponentInChildren <Cannon> ();
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
