using UnityEngine;
using System.Collections;

public class SplashMenus : MonoBehaviour 
{
	static Transform screensHandle;

	enum Steps { login, selection, customization, lobby };

	static Steps _currentStep;
	public static Steps currentStep
	{
		get { return _currentStep; }
		set 
		{
			_currentStep = value;
		}
	}

	void Awake ()
	{
		screensHandle = GameObject.Find ("Screens Handle").GetComponent <Transform> ();
	}
}
