using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour 
{
	EnabledTransitionPosition bgBotTrans;
	EnabledTransitionScale bgMidTrans;
	EnabledTransitionScale confirmTrans;
	EnabledTransitionScale loginTrans;
	EnabledTransitionScale createTrans;

	public static InputField usernameField;

	InputField passwordField;
	InputField confirmField;

	void Awake ()
	{
		// References setup
		bgBotTrans = GameObject.Find ("Background Bot").GetComponent<EnabledTransitionPosition> ();
		bgMidTrans = GameObject.Find ("Background Mid").GetComponent<EnabledTransitionScale> ();
		confirmTrans = GameObject.Find ("Field Confirm").GetComponent<EnabledTransitionScale> ();
		loginTrans = GameObject.Find ("Button Login").GetComponent <EnabledTransitionScale> ();
		createTrans = GameObject.Find ("Button Create").GetComponent <EnabledTransitionScale> ();

		passwordField = GameObject.Find ("Text Field Password").GetComponent <InputField> ();
		confirmField = GameObject.Find ("Text Field Confirm").GetComponent <InputField> ();

		usernameField = GameObject.Find ("Text Field Username").GetComponent<InputField> ();

		// Character validation
		usernameField.characterValidation = 
		passwordField.characterValidation = 
		confirmField.characterValidation  =  InputField.CharacterValidation.Alphanumeric;

	}

	string lastEnteredUsername = "";
	public void HandleUsernameExistance (string username)
	{
		print ("handling existence of " + username);
		// TODO: when pressing button check username again

		passwordField.Select ();

		if (username != lastEnteredUsername)
			ClearPasswords ();
		passwordField.Select ();
		passwordField.ActivateInputField ();

		if (!Server.UsernameExists (username)) {		
			bgMidTrans.StartGrowing (() => { });
			bgBotTrans.StartExpanding (GrowConfirmAndCreate);
			loginTrans.StartShrinking (() => { });
		} 
		else {
			createTrans.StartShrinking (() => { });
			StartCoroutine (WaitAndShrinkConfirm (EnabledTransitionScale.DURATION / 3f));
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
		StartCoroutine (WaitAndGrowCreate (EnabledTransitionScale.DURATION / 3f));

	}

	IEnumerator WaitAndGrowCreate (float duration)
	{
		yield return new WaitForSeconds (duration);
		createTrans.StartGrowing (DoneTransToCreate);
	}

	void DoneTransToCreate ()
	{
		print ("done transition to create");
	}



	// Transition to Login
	IEnumerator WaitAndShrinkConfirm (float duration)
	{
		yield return new WaitForSeconds (duration);
		confirmTrans.StartShrinking (CollapseBottomAndShrinkMiddle);
	}

	void CollapseBottomAndShrinkMiddle ()
	{
		bgBotTrans.StartCollapsing (() => { });
		bgMidTrans.StartShrinking (GrowLogin);
	}

	void GrowLogin ()
	{
		loginTrans.StartGrowing (DoneTransToLogin);
	}

	void DoneTransToLogin ()
	{
		print ("done transition to login");
	}
	
}
