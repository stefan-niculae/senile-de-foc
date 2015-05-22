using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankSelection : MonoBehaviour 
{
	public Sprite availableBackground;
	public Sprite pickedBackground;
	public Sprite unavailableBackground;

	Image[] backgrounds;
	bool[] available;

	Button lockInButton;
	Button customizeButton;
	int pickedNumber;


	Transition lockinButtonTrans;
	Transition customizeButtonTrans;

	void Awake ()
	{Debug.Log ("Server simulation: 0-5 to enable/disable tank types");
		backgrounds = new Image[5];
		backgrounds [0] = GameObject.Find ("Calm Background")	.GetComponent <Image> ();
		backgrounds [1] = GameObject.Find ("Heavy Background")	.GetComponent <Image> ();
		backgrounds [2] = GameObject.Find ("Angry Background")	.GetComponent <Image> ();
		backgrounds [3] = GameObject.Find ("Sneaky Background")	.GetComponent <Image> ();
		backgrounds [4] = GameObject.Find ("Custom Background")	.GetComponent <Image> ();

		lockInButton = GameObject.Find ("Lock In Button").GetComponent <Button> ();

		lockinButtonTrans = GameObject.Find ("Lock In Button").GetComponent <Transition> ();
		customizeButtonTrans = GameObject.Find ("Customize Button").GetComponent <Transition> ();

		lockinButtonTrans		.Initialize (Transition.Properties.scale, 	Vector3.one, 	Constants.SMALL_DURATION);
		customizeButtonTrans	.Initialize (Transition.Properties.scale, Vector3.zero, Constants.SMALL_DURATION);


		available = new bool[5];
		for (int i = 0; i < 5; i++)
			SetAvailability (i, true);

		Reset ();
	}

	public void Reset ()
	{
		lockInButton.interactable = false;
		SetAvailability (pickedNumber, true);
	}
	
	public void SetAvailability (int number, bool value)
	{
		available [number] = value;
		backgrounds [number].sprite = value ? availableBackground : unavailableBackground;
	}

	public void Pick (int number)
	{
		if (available [number]) {
			pickedNumber = number;

			if (number == 4)
				ShowCustomize ();
			else
				ShowLockin ();
		
			Server.SelectTank (number);
			SplashMenus.currentTankType = number;
			
			// TODO make sure to remove this when the server will be implemented to avoid duplication
			GameObject.Find("Menu Logic").GetComponent<WaitingLobby>(). AddUser (SplashMenus.currentUsername, SplashMenus.currentTankType);


			backgrounds [number].sprite = pickedBackground;
			for (int i = 0; i < 5; i++)
				if (i != number)
					backgrounds [i].sprite = available [i] ? availableBackground : unavailableBackground;
		}
	}

	void ShowCustomize ()
	{
		lockInButton.interactable = false;
		lockinButtonTrans.TransitionTo (Vector3.zero, callback: () => { 
			customizeButtonTrans.TransitionTo (Vector3.one);
		});
	}

	void ShowLockin ()
	{
		lockInButton.interactable = true;
		customizeButtonTrans.TransitionTo (Vector3.zero, callback: () => { 
			lockinButtonTrans.TransitionTo (Vector3.one);
		});
	}

	public void Lockin ()
	{
		SplashMenus.currentStep = SplashMenus.Steps.lobby;
	}

	public void Customize ()
	{
		SplashMenus.currentStep = SplashMenus.Steps.customization;
	}

	void Update ()
	{
		//TODO move this to a thread listener (for disable selection messages from the server)?

		if (Input.GetKeyDown (KeyCode.Alpha0))
			SetAvailability (0, !available [0]);
		if (Input.GetKeyDown (KeyCode.Alpha1))
			SetAvailability (1, !available [1]);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			SetAvailability (2, !available [2]);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			SetAvailability (3, !available [3]);
		if (Input.GetKeyDown (KeyCode.Alpha4))
			SetAvailability (4, !available [4]);
	}
}
