using UnityEngine;
using System.Collections;
using System;

public class Server : Singleton<Server>
{
	static NetworkView netView;
	TankSelection tankSelection;

	void Awake ()
	{
		netView = Instance.GetComponent <NetworkView> ();
//		PlayerPrefs.DeleteAll ();

		tankSelection = GameObject.Find ("Menu Logic").GetComponent <TankSelection> ();
	}


	// Login/Register
	static Action<bool> onUsernameExistanceReceival;
	public static void UsernameExists (string username, Action<bool> onReceival)
	{
		onUsernameExistanceReceival = onReceival;
		netView.RPC ("RequestUsernameExistance", RPCMode.Server, username);
	}
	[RPC]
	void RequestUsernameExistance (string username)
	{
		NetworkStatus.Show ("Checking if username exists", NetworkStatus.MessageType.working);
	}
	[RPC]
	void ReceiveUsernameExistance (bool value)
	{
		onUsernameExistanceReceival (value);
		if (value)
			NetworkStatus.Show ("Username exists", NetworkStatus.MessageType.success);
		else
			NetworkStatus.Show ("Username doesn't exist, create it now", NetworkStatus.MessageType.success);
	}
	
	public static void CreateUser (string username, string password)
	{
		netView.RPC ("SendCreateUser", RPCMode.Server, username, password);
	}
	[RPC]
	void SendCreateUser (string username, string password)
	{
		print ("creating " + username + ", " + password);
	}

	static Action<bool> onPasswordMatchReceival;
	public static void PasswordMatches (string username, string password, Action<bool> onReceival)
	{
		onPasswordMatchReceival = onReceival;
		netView.RPC ("RequestPasswordMatch", RPCMode.Server, username, password);
	}
	[RPC]
	void RequestPasswordMatch (string username, string password)
	{
		NetworkStatus.Show ("Checking if password is correct", NetworkStatus.MessageType.working);
	}
	[RPC]
	void ReceivePasswordMatch (bool value)
	{
		onPasswordMatchReceival (value);
		if (value)
			NetworkStatus.Show ("Password ok", NetworkStatus.MessageType.success);
		else
			NetworkStatus.Show ("Password incorrect", NetworkStatus.MessageType.failure);
	}


	public static void Login (string username, string password)
	{
		NetworkStatus.Show ("Logged in as " + username, NetworkStatus.MessageType.success);
		netView.RPC ("SendLogin", RPCMode.Server, username);
	}
	[RPC]
	void SendLogin (string username)
	{
		NetworkStatus.Show ("Logging in", NetworkStatus.MessageType.working);
	}

	public static void Logout ()
	{
		//again, not sure this is needed
		print ("logged out");
	}


	// Tank Select
	public static void SelectTank (int slotNr, int body, int barrel, int primary, int secondary,
	                               int damage, int rate, int armor,  int speed)
	{
		// TODO signal to others to disable this selection
		// TODO also when a new player logs in, send the list of disabled tanks
		netView.RPC ("SendSelectTank", RPCMode.Server, slotNr, body, barrel, primary, secondary,
		             				   				   damage, rate, armor,  speed);
	}
	[RPC]
	void SendSelectTank (int slotNr, int body, int barrel, int primary, int secondary,
	                     int damage, int rate, int armor,  int speed)
	{
		print ("selected tank " + slotNr);
	}

	[RPC]
	public void ReceiveDisabledOption (int slotNr)
	{
		print ("disabling slot nr " + slotNr);
		tankSelection.DisableOption (slotNr);
	}


	// Lobby
	public static void RegisterReady (string username)
	{
		print (username + " is ready");
	}





	// Game
}
