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


	Transition lockinButtonTrans;
	Transition customizeButtonTrans;

	void Awake ()
	{
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
		SetAvailability (SplashMenus.currentTankType, true);
		ShowLockin ();
	}

	public void SetAvailability (int number, bool value)
	{
		// The tank selected by this is managed internally
		if (number == SplashMenus.currentTankType)
			return;

		// Can't disable the custom tank
		if (value == false && number == 4)
			return;
		print ("setting " + number + " to " + value); 
		available [number] = value;
		backgrounds [number].sprite = value ? availableBackground : unavailableBackground;
	}

	public void Pick (int number)
	{
		if (available [number]) {
			SplashMenus.currentTankType = number;

			if (number == 4)
				ShowCustomize ();
			else
				ShowLockin ();

			TankType type = new TankType (number);
			Rates rates = new Rates (number);
			SplashServer.SelectTankType (type);
			SplashServer.SelectRates (rates);


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
}
