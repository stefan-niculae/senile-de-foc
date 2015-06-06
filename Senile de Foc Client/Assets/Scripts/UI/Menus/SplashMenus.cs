using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashMenus : MonoBehaviour 
{
	public static string currentUsername;
	public static int currentTankType = NetworkConstants.NOT_SET;

	static readonly float transitionDuration = .5f;
	static Transition screenTransition;

	public enum Steps { login, selection, customization, lobby };

	static Steps _currentStep;
	public static Steps currentStep
	{
		get { return _currentStep; }
		set 
		{
			_currentStep = value;

			Vector3 destination = Vector3.zero;
			switch (value) 
			{
				case Steps.login:
					break;

				case Steps.selection: 
					destination.x = 1 * width;
					break;

				case Steps.customization:
					destination.x = 1 * width;
					destination.y =-1 * height;
					break;

				case Steps.lobby:
					destination.x = 2 * width;
					break;
			}
			// Times -1 because we move the screen handle, not the screen
			screenTransition.TransitionTo  (-1 * destination);
		}
	}

	static float width, height;

	void Awake ()
	{
		screenTransition = GameObject.Find ("Screens Handle").GetComponent <Transition> ();
		screenTransition.Initialize (Transition.Properties.position, Vector3.zero, transitionDuration);

		currentStep = Steps.login;

		Vector2 refRes = GameObject.Find ("Canvas").GetComponent <CanvasScaler> ().referenceResolution;
		width = refRes.x;
		height = refRes.y;

		currentUsername = "";
		currentTankType = -1;
	}
}
