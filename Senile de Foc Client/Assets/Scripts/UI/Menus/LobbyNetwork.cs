using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LobbyNetwork : Singleton<LobbyNetwork> 
{
	const string 
		IP_KEY = "preffered ip",
		PORT_KEY = "preffered port";

	[HideInInspector] public string prefferedIP
	{
		get { return PlayerPrefs.GetString (IP_KEY, NetworkConstants.DEFAULT_IP); }
		set { PlayerPrefs.SetString (IP_KEY, value); }
	}

	[HideInInspector] public int prefferedPort
	{
		get { return PlayerPrefs.GetInt (PORT_KEY, NetworkConstants.DEFAULT_PORT); }
		set { PlayerPrefs.SetInt (PORT_KEY, value); }
	}

	InputField ipField, portField;

	string enteredIP
	{
		get { return ipField.text; }
		set { ipField.text = value; }
	}

	int enteredPort
	{
		get { return int.Parse (portField.text); }
		set { portField.text = value.ToString (); }
	}

	Transition frameTransition;
	public Vector2 hiddenPosition;
	bool pointerInside;
	public bool PointerInside
	{
		get { return pointerInside; }	
		set { pointerInside = value; }
	}
	bool frameShown;

	void Awake ()
	{
		ipField 	= GameObject.Find ("IP Field").GetComponent <InputField> ();
		portField 	= GameObject.Find ("Port Field").GetComponent <InputField> ();

		portField.characterValidation = InputField.CharacterValidation.Integer;

		frameTransition = GameObject.Find ("Network Frame").GetComponent <Transition> ();
		frameTransition.Initialize (Transition.Properties.position, 
									hiddenPosition, 
									Constants.SMALL_DURATION);

		PointerInside = false;
		frameShown = false;
	}

	void Start ()
	{
		enteredPort = prefferedPort;
		enteredIP = prefferedIP;
	}

	public void ValidateIP ()
	{
		// Only valid ip addresses
		if (!Regex.IsMatch (enteredIP, @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
			ResetIP ();
		else
			prefferedIP = enteredIP;
	}

	public void ValidatePort ()
	{
		if (enteredPort < 1 || enteredPort > (1 << 16) - 1)
			ResetPort ();
		else
			prefferedPort = enteredPort;
	}

	public void ShowFrame ()
	{
		frameTransition.TransitionTo (Vector3.zero, callback: () => frameShown = true );
	}

	public void HideFrame ()
	{
		frameTransition.TransitionTo (hiddenPosition, callback: () => frameShown = false );
	}

	public void ResetIP ()
	{
		enteredIP = NetworkConstants.DEFAULT_IP;
		prefferedIP = enteredIP;
	}

	public void ResetPort ()
	{
		enteredPort = NetworkConstants.DEFAULT_PORT;
		prefferedPort = enteredPort;
	}

	void Update ()
	{
		// On click outside
		if (Input.GetMouseButton (0) && frameShown && !PointerInside)
			HideFrame ();
	}
}
