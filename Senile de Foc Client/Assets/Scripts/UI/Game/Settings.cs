using UnityEngine;
using System.Collections;

public class Settings : Singleton<Settings> 
{
	bool isExpanded = false;

	public Vector3 deltaPos;

	Transition bandTransition;
	Vector3 upPos;
	Vector3 downPos;

	void Awake ()
	{
		bandTransition = Utils.childWithName (transform, "Band").GetComponent <Transition> ();

		downPos = bandTransition.transform.localPosition;
		upPos = downPos + deltaPos;
		bandTransition.Initialize (Transition.Properties.position, upPos, Constants.SMALL_DURATION);
	}

	public void Toggle ()
	{
		if (isExpanded)
			Collapse ();
		else
			Expand ();
	}

	void Expand ()
	{
		bandTransition.TransitionTo (downPos, callback: () => isExpanded = true);
	}

	void Collapse ()
	{
		bandTransition.TransitionTo (upPos, callback: () => isExpanded = false);
	}

	void Update ()
	{
		if ((Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1)) && isExpanded)
			Collapse ();
	}


}
