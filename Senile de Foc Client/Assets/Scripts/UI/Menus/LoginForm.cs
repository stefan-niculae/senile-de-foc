using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour 
{
	PositionTransition bgBotTrans;
	ScaleTransition bgMidTrans;
	ScaleTransition confirmTrans;
	ScaleTransition loginTrans;
	ScaleTransition createTrans;

	InputField usernameField;
	InputField passwordField;
	InputField confirmField;

	Button loginButton;
	Button createButton;

	Toggle rememberToggle;


	PositionTransition splashScreen;
	PositionTransition selectScreen;

	
	string savedUserKey = "savedUser";
	string savedUser
	{
		get { return PlayerPrefs.GetString (savedUserKey, ""); }
		set { 		 PlayerPrefs.SetString (savedUser, value); }
	}

	// TODO switch from plain text password to hashcode storing
	string savedPassKey = "savedUser";
	string savedPass
	{
		get { return PlayerPrefs.GetString (savedPassKey, ""); }
		set { 		 PlayerPrefs.SetString (savedPass, value); }
	}

	bool creating = false;

	void Awake ()
	{
		// References setup
		bgBotTrans = GameObject.Find ("Background Bot").GetComponent<PositionTransition> ();
		bgMidTrans = GameObject.Find ("Background Mid").GetComponent<ScaleTransition> ();
		confirmTrans = GameObject.Find ("Field Confirm").GetComponent<ScaleTransition> ();
		loginTrans = GameObject.Find ("Button Login").GetComponent <ScaleTransition> ();
		createTrans = GameObject.Find ("Button Create").GetComponent <ScaleTransition> ();

		passwordField = GameObject.Find ("Text Field Password").GetComponent <InputField> ();
		confirmField = GameObject.Find ("Text Field Confirm").GetComponent <InputField> ();
		usernameField = GameObject.Find ("Text Field Username").GetComponent<InputField> ();

		loginButton = GameObject.Find ("Button Login").GetComponent <Button> ();
		createButton = GameObject.Find ("Button Create").GetComponent <Button> ();

		rememberToggle = GameObject.Find ("Remember Toggle").GetComponent <Toggle> ();

		splashScreen = GameObject.Find ("Splash Screen").GetComponent <PositionTransition> ();
		selectScreen = GameObject.Find ("Selection Screen").GetComponent <PositionTransition> ();

		// Character validation
		usernameField.characterValidation = 
		passwordField.characterValidation = 
		confirmField.characterValidation  =  InputField.CharacterValidation.Alphanumeric;
	}

	void Start ()
	{
		usernameField.text = savedUser;
		passwordField.text = savedPass;
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
			bgMidTrans.StartGrowing (() => { });
			bgBotTrans.StartMoving (GrowConfirmAndCreate);
			loginTrans.StartShrinking (() => { });

			creating = true;
		} 
		else {
			createTrans.StartShrinking (() => { });
			StartCoroutine (WaitAndShrinkConfirm (ScaleTransition.DURATION / 3f));

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
		confirmTrans.StartGrowing (() => { });
		StartCoroutine (WaitAndGrowCreate (ScaleTransition.DURATION / 3f));

	}

	IEnumerator WaitAndGrowCreate (float duration)
	{
		yield return new WaitForSeconds (duration);
		createTrans.StartGrowing (DoneTransToCreate);
	}

	void DoneTransToCreate ()
	{
		//print ("done transition to create");
	}



	// Transition to Login
	IEnumerator WaitAndShrinkConfirm (float duration)
	{
		yield return new WaitForSeconds (duration);
		confirmTrans.StartShrinking (CollapseBottomAndShrinkMiddle);
	}

	void CollapseBottomAndShrinkMiddle ()
	{
		bgBotTrans.StartMovingBack (() => { });
		bgMidTrans.StartShrinking (GrowLogin);
	}

	void GrowLogin ()
	{
		loginTrans.StartGrowing (DoneTransToLogin);
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

			if (rememberToggle.isOn) {
				savedUser = usernameField.text;
				savedPass = passwordField.text;
			}
			// No else here, don't clear the saved user if a guest enters!

			splashScreen.StartMoving (() => { }, .65f);
			selectScreen.StartMoving (() => { }, .65f);
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
}
