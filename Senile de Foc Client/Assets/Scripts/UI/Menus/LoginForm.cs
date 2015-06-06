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


	string enteredUsername
	{
		get { return usernameField.text.ToUpper (); }
		set { usernameField.text = value.ToUpper (); }
	}



	Toggle rememberToggle;

	string SAVED_USER_KEY = "savedUser";
	string savedUser
	{
		get { return PlayerPrefs.GetString (SAVED_USER_KEY, ""); }
		set { 		 PlayerPrefs.SetString (SAVED_USER_KEY, value); }
	}

	// TODO (?) store hashcodes not plain text
	string SAVED_PASS_KEY = "savedPass";
	string savedPass
	{
		get { return PlayerPrefs.GetString (SAVED_PASS_KEY, ""); }
		set { 		 PlayerPrefs.SetString (SAVED_PASS_KEY, value); }
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
		bgBotUp 	= bgBotTrans.transform.localPosition;
		bgBotDown 	= bgBotUp + new Vector3 (0, -89, 0);
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
//		PlayerPrefs.DeleteKey (SAVED_USER_KEY);
//		PlayerPrefs.DeleteKey (SAVED_PASS_KEY);

		enteredUsername = savedUser;
		passwordField.text = savedPass;
		loginButton.Select ();
	}

	string lastEnteredUsername = "";
	public void HandleUsernameExistance (string username)
	{
		passwordField.Select ();
		username = enteredUsername;
		if (username != lastEnteredUsername)
			ClearPasswords ();
		passwordField.Select ();
		passwordField.ActivateInputField ();

		// Requesting username existance status
		SplashServer.UsernameExists (username, UsernameExistanceReceival);

		lastEnteredUsername = enteredUsername;
	}

	void UsernameExistanceReceival (bool value)
	{
		if (!value)
			ShowCreate ();
		else
			ShowLogin ();
	}

	void ShowCreate ()
	{
		bgMidTrans.TransitionTo (bgMidBig);
		
		bgBotTrans.TransitionTo (bgBotDown, callback: GrowConfirmAndCreate);
		loginTrans.TransitionTo (Vector3.zero);
		
		creating = true;
	}

	void ShowLogin ()
	{
		createTrans.TransitionTo (Vector3.zero);
		StartCoroutine (WaitAndShrinkConfirm (Constants.SMALL_DURATION / 3f));
		
		creating = false;
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
		
		SplashServer.UsernameExists (enteredUsername, UsernameCheckReceival);
	}

	void UsernameCheckReceival (bool exists)
	{
		if (!exists) {
			NetworkStatus.Show ("Saved username no longer exists, re-create it", NetworkStatus.MessageType.failure);
			usernameField.Select ();
		}
		else {
			foreach (var player in WaitingLobby.currentPlayers)
				if (player.name == enteredUsername) {
					NetworkStatus.Show ("User already logged in", NetworkStatus.MessageType.failure);
					usernameField.Select ();
					return;
				}
		
			SplashServer.PasswordMatches (enteredUsername, passwordField.text, PasswordMatchesReceival);
		}
	}


	void PasswordMatchesReceival (bool value)
	{
		if (value) {
			SplashServer.Login (enteredUsername, passwordField.text);
			SplashMenus.currentUsername = enteredUsername;
			
			if (rememberToggle.isOn) {
				savedUser = enteredUsername;
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
			SplashServer.CreateUser (enteredUsername, passwordField.text);
			ShowLogin ();
			HandleLogin ();
		} 
		else
			confirmField.Select ();
	}

	bool UserOrPassEmpty ()
	{
		if (enteredUsername == "") {
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
		SplashServer.Logout ();
		SplashMenus.currentStep = SplashMenus.Steps.login;
		tankSelection.Reset ();
		TankCustomization.Reset ();
		loginButton.Select ();
	}

	public void ExitGame ()
	{
		Logout ();
		Application.Quit ();
	}
}
