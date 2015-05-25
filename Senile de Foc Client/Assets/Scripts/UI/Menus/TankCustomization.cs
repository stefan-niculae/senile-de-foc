using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankCustomization : MonoBehaviour 
{
	Text remainingText;
	int _remaining;
	static public int remaining
	{
		get { return instance._remaining; }
		set 
		{
			instance._remaining = value;
			instance.remainingText.text = "Remaining: " + value;
			instance.DoneCheck ();
		}
	}

	int customCannon, customSpecial;

	Button doneButton;
	Image[] cannonButtons;
	Image[] specialButtons;

	public Sprite deselected;
	public Sprite selected;

	AttributeBeans customizationBeans;
	static TankCustomization instance;


	public Sprite[] barrelPreviews;
	Image barrelPreview;


	public Sprite[] cannonIcons;
	public Sprite[] specialIcons;
	Image selectionCannon, selectionSpecial;

	GameObject selectionHint;
	AttributeBeans selectionBeans;


	void Awake ()
	{
		instance = this;

		remainingText = GameObject.Find ("Remaining Text").GetComponent <Text> ();
		doneButton = GameObject.Find ("Done Button").GetComponent <Button> ();
		customizationBeans = GameObject.Find ("Customization Attributes").GetComponent <AttributeBeans> ();
		barrelPreview = GameObject.Find ("Custom Barrel").GetComponent <Image> ();
		selectionCannon = GameObject.Find ("Custom Cannon Icon").GetComponent <Image> ();
		selectionSpecial = GameObject.Find ("Custom Special Icon").GetComponent <Image> ();
		selectionBeans = GameObject.Find ("Custom Attributes").GetComponent <AttributeBeans> ();
		selectionHint = GameObject.Find ("Attributes Hint");

		cannonButtons = new Image [2];
		for (int i = 0; i < cannonButtons.Length; i++)
			cannonButtons [i] = GameObject.Find ("Cannon " + i).GetComponent <Image> ();

		specialButtons = new Image [4];
		for (int i = 0; i < specialButtons.Length; i++)
			specialButtons [i] = GameObject.Find ("Special " + i).GetComponent <Image> ();
	}

	void Start ()
	{
		Reset ();
	}
	
	void DoneCheck ()
	{
		bool isDone = (remaining == 0 && customCannon != -1 && customSpecial != -1);
		doneButton.interactable = isDone;

		selectionBeans.gameObject.SetActive (remaining == 0);
		selectionHint.SetActive (remaining != 0);
		if (remaining == 0)
			selectionBeans.UpdateAll (customizationBeans.values);
	}

	public void BackToSelection ()
	{
		SplashMenus.currentStep = SplashMenus.Steps.selection;
	}

	public void Lockin ()
	{
		TankType type = new TankType (5, 
		                              5, 
		                              5 + customCannon, 
		                              customCannon, 
		                              customSpecial);
		Rates rates = new Rates (
									customizationBeans.values [0],
									customizationBeans.values [1],
									customizationBeans.values [2],
									customizationBeans.values [3] );
		Server.SelectTankType (type);
		Server.SelectRates (rates);

		SplashMenus.currentStep = SplashMenus.Steps.lobby;
	}

	public void PickCannon (int type)
	{
		customCannon = type;
		for (int i = 0; i < cannonButtons.Length; i++)
			cannonButtons [i].sprite = (i == type ? selected : deselected);
		DoneCheck ();

		if (type == -1)
			type = barrelPreviews.Length - 1;
		barrelPreview.sprite = barrelPreviews [type];
		selectionCannon.sprite = cannonIcons [type];
	}

	public void PickSpecial (int type)
	{
		customSpecial = type;
		for (int i = 0; i < specialButtons.Length; i++)
			specialButtons [i].sprite = (i == type ? selected : deselected);
		DoneCheck ();
	
		if (type == -1)
			type = specialIcons.Length - 1;
		selectionSpecial.sprite = specialIcons [type];
	}

	public static void Reset ()
	{
		remaining = Constants.PTS_TO_SPEND;
		instance.PickCannon (-1);
		instance.PickSpecial (-1);
		instance.customizationBeans.Reset ();
	}
}
