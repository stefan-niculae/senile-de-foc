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

	void Awake ()
	{
		backgrounds = new Image[5];
		backgrounds [0] = GameObject.Find ("Calm Background")	.GetComponent <Image> ();
		backgrounds [1] = GameObject.Find ("Heavy Background")	.GetComponent <Image> ();
		backgrounds [2] = GameObject.Find ("Angry Background")	.GetComponent <Image> ();
		backgrounds [3] = GameObject.Find ("Sneaky Background")	.GetComponent <Image> ();
		backgrounds [4] = GameObject.Find ("Custom Background")	.GetComponent <Image> ();

		available = new bool[5];
		for (int i = 0; i < 5; i++)
			SetAvailability (i, true);
	}
	
	public void SetAvailability (int number, bool value)
	{
		available [number] = value;
		backgrounds [number].sprite = value ? availableBackground : unavailableBackground;
	}

	public void Pick (int number)
	{
		if (available [number]) {
		
			Server.SelectTank (number);

			backgrounds[number].sprite = pickedBackground;
			for (int i = 0; i < 5; i++)
				if (i != number)
					backgrounds [i].sprite = available [i] ? availableBackground : unavailableBackground;
		}
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
