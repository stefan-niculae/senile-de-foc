using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour 
{
	Transition bgBotTrans;
	Vector3 bgBotUp;
	Vector3 bgBotDown;

	Transition bgMidTrans;
	Vector3 bgMidSmall 	= new Vector3 (1, 0, 1);
	Vector3 bgMidBig = Vector3.one;

	Transition confirmTrans;
	Transition loginTrans;
	Transition createTrans;


	InputField usernameField;
	InputField passwordField;
	InputField confirmField;

	Button loginButton;
	Button createButton;



	Toggle rememberToggle;

	string savedUserKey = "savedUser";
	string savedUser
	{
		get { return PlayerPrefs.GetString (savedUserKey, ""); }
		set { 		 PlayerPrefs.SetString (savedUserKey, value); }
	}

	// TODO switch from plain text password to hashcode storing
	string savedPassKey = "savedPass";
	string savedPass
	{
		get { return PlayerPrefs.GetString (savedPassKey, ""); }
		set { 		 PlayerPrefs.SetString (savedPassKey, value); }
	}

	bool creating = false;



	TankSelection tankSelection;


	void Awake ()
	{
		// References setup
		bgBotTrans 		= GameObject.Find ("Background Bot")		.GetComponent <Transition> ();
		bgMidTrans 		= GameObject.Find ("Background Mid")		.GetComponent <Transition> ();
		confirmTrans 	= GameObject.Find ("Field Confirm")			.GetComponent <Transition> ();
		loginTrans 		= GameObject.Find ("Button Login")			.GetComponent <Transition> ();
		createTrans 	= GameObject.Find ("Button Create")			.GetComponent <Transition> ();

		passwordField 	= GameObject.Find ("Text Field Password")	.GetComponent <InputField> ();
		confirmField 	= GameObject.Find ("Text Field Confirm")	.GetComponent <InputField> ();
		usernameField 	= GameObject.Find ("Text Field Username")	.GetComponent <InputField> ();

		loginButton 	= GameObject.Find ("Button Login")			.GetComponent <Button> ();
		createButton 	= GameObject.Find ("Button Create")			.GetComponent <Button> ();

		rememberToggle 	= GameObject.Find ("Remember Toggle")		.GetComponent <Toggle> ();

		tankSelection   = GameObject.Find ("Menu Logic")			.GetComponent <TankSelection> ();


		// Transition initialization
		bgBotUp = bgBotTrans.transform.localPosition;
		bgBotDown = bgBotUp + new Vector3 (0, -89, 0);
		bgBotTrans	.Initialize (Transition.Properties.position, 	bgBotUp, 		Constants.SMALL_DURATION);


		bgMidTrans	.Initialize (Transition.Properties.scale, 		bgMidSmall, 	Constants.SMALL_DURATION);
		confirmTrans.Initialize (Transition.Properties.scale,		Vector3.zero,	Constants.SMALL_DURATION);
		createTrans	.Initialize (Transition.Properties.scale,		Vector3.zero,	Constants.SMALL_DURATION);
		loginTrans	.Initialize (Transition.Properties.scale, 		Vector3.one,	Constants.SMALL_DURATION);


		// Character validation
		usernameField.characterValidation = 
		passwordField.characterValidation = 
		confirmField.characterValidation  =  InputField.CharacterValidation.Alphanumeric;
	}

	void Start ()
	{
		usernameField.text = savedUser;
		passwordField.text = savedPass;
		loginButton.Select ();
	}

	string lastEnteredUsername = "";
	public void HandleUsernameExistance (string username)
	{
		passwordField.Select ();

		if (username != lastEnteredUsername)
			ClearPasswords ();
		passwordField.Select ();
		passwordField.ActivateInputField ();

		if (!Server.UsernameExists (username)) {	
			bgMidTrans.TransitionTo (bgMidBig);

			bgBotTrans.TransitionTo (bgBotDown, callback: GrowConfirmAndCreate);
			loginTrans.TransitionTo (Vector3.zero);

			creating = true;
		} 
		else {
			createTrans.TransitionTo (Vector3.zero);
			StartCoroutine (WaitAndShrinkConfirm (Constants.SMALL_DURATION / 3f));

			creating = false;
		}

		lastEnteredUsername = username;
	}

	void ClearPasswords ()
	{
		passwordField.text = "";
		confirmField.text = "";
	}


	// Transition to Create
	void GrowConfirmAndCreate ()
	{
		confirmTrans.TransitionTo (Vector3.one);
		StartCoroutine (WaitAndGrowCreate (Constants.SMALL_DURATION / 3f));

	}

	IEnumerator WaitAndGrowCreate (float duration)
	{
		yield return new WaitForSeconds (duration);
		createTrans.TransitionTo (Vector3.one, callback: DoneTransToCreate);
	}

	void DoneTransToCreate ()
	{
		//print ("done transition to create");
	}



	// Transition to Login
	IEnumerator WaitAndShrinkConfirm (float duration)
	{
		yield return new WaitForSeconds (duration);
		confirmTrans.TransitionTo (Vector3.zero, callback: CollapseBottomAndShrinkMiddle);
	}

	void CollapseBottomAndShrinkMiddle ()
	{
		bgBotTrans.TransitionTo (bgBotUp);
		bgMidTrans.TransitionTo (bgMidSmall, callback: GrowLogin);
	}

	void GrowLogin ()
	{
		loginTrans.TransitionTo (Vector3.one, callback: DoneTransToLogin);
	}

	void DoneTransToLogin ()
	{
		//print ("done transition to login");
	}





	public void EnteredPassword ()
	{
		if (creating)
			confirmField.Select ();
		else {
			loginButton.Select ();
			HandleLogin ();
		}
	}

	public void EnteredConfirm ()
	{
		createButton.Select ();
		HandleCreate ();
	}

	public void PressedLogin ()
	{
		HandleLogin ();
	}

	public void PressedCreate ()
	{
		HandleCreate ();
	}

	void HandleLogin ()
	{
		if (UserOrPassEmpty ())
			return;

		if (Server.PasswordMatches (usernameField.text, passwordField.text)) {
			Server.Login (usernameField.text, passwordField.text);
			SplashMenus.currentUsername = usernameField.text;

			if (rememberToggle.isOn) {
				savedUser = usernameField.text;
				savedPass = passwordField.text;
			}
			// No else here, don't clear the saved user if a guest enters!

			SplashMenus.currentStep = SplashMenus.Steps.selection;
		}
		else 
			passwordField.Select ();

	}

	void HandleCreate ()
	{
		if (UserOrPassEmpty ())
			return;

		if (passwordField.text == confirmField.text) {
			Server.CreateUser (usernameField.text, passwordField.text);
			HandleLogin ();
		} 
		else
			confirmField.Select ();
	}

	bool UserOrPassEmpty ()
	{
		if (usernameField.text == "") {
			usernameField.Select ();
			return true;
		}

		if (passwordField.text == "") {
			passwordField.Select ();
			return true;
		}

		return false;
	}

	public void Logout ()
	{
		Server.Logout ();
		SplashMenus.currentStep = SplashMenus.Steps.login;
		tankSelection.Reset ();
		loginButton.Select ();
	}
}
