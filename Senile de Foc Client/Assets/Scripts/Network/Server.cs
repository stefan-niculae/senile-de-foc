using UnityEngine;
using System.Collections;
using System;

public class Server : Singleton<Server>
{
	static NetworkView netView;
	TankSelection tankSelection;
	WaitingLobby waitingLobby;

	void Awake ()
	{
		netView = Instance.GetComponent <NetworkView> ();
//		PlayerPrefs.DeleteAll ();

		tankSelection = GameObject.Find ("Menu Logic").GetComponent <TankSelection> ();
		waitingLobby = GameObject.Find ("Menu Logic").GetComponent <WaitingLobby> ();
	}


	// Login/Register
	static Action<bool> onUsernameExistanceReceival;
	public static void UsernameExists (string username, Action<bool> onReceival)
	{
		NetworkStatus.Show ("Checking if " + username + " exists", NetworkStatus.MessageType.working);
		onUsernameExistanceReceival = onReceival;
		netView.RPC ("RequestUsernameExistance", RPCMode.Server, username);
	}
	[RPC]
	void RequestUsernameExistance (string username)
	{ }
	[RPC]
	public void ReceiveUsernameExistance (bool value)
	{
		onUsernameExistanceReceival (value);
		if (value)
			NetworkStatus.Show ("Username exists", NetworkStatus.MessageType.success);
		else
			NetworkStatus.Show ("Username doesn't exist, create it now", NetworkStatus.MessageType.success);
	}
	
	public static void CreateUser (string username, string password)
	{
		NetworkStatus.Show ("Created " + username, NetworkStatus.MessageType.success);
		netView.RPC ("SendCreateUser", RPCMode.Server, username, password);
	}
	[RPC]
	void SendCreateUser (string username, string password)
	{ }

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
	public void ReceivePasswordMatch (bool value)
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
	{ }

	public static void Logout ()
	{
		NetworkStatus.Show ("Logged out", NetworkStatus.MessageType.success);
	}
	[RPC]
	void SendLogout ()
	{ }


	// Tank Select
	public static void SelectTankType (TankType type)
	{
		NetworkStatus.Show ("Sent tank selection " + type.slotNr, NetworkStatus.MessageType.success);
		netView.RPC ("SendTankType",
		             RPCMode.Server,
		             NetworkUtils.ObjectToByteArray (type));
	}
	[RPC]
	void SendTankType (byte[] tankType)
	{ }

	public static void SelectRates (Rates rates)
	{
		NetworkStatus.Show ("Sent attribute rates", NetworkStatus.MessageType.success);
		netView.RPC ("SendRates",
		             RPCMode.Server,
		             NetworkUtils.ObjectToByteArray (rates));
	}
	[RPC]
	void SendRates (byte[] rates)
	{ }

	[RPC]
	public void ReceiveDisableOption (bool taken0, bool taken1, bool taken2, bool taken3)
	{
		tankSelection.SetAvailability (0, taken0);
		tankSelection.SetAvailability (1, taken1);
		tankSelection.SetAvailability (2, taken2);
		tankSelection.SetAvailability (3, taken3);
	}


	// Lobby
	public static void RegisterReady ()
	{
		NetworkStatus.Show ("Sent ready signal", NetworkStatus.MessageType.success);
		netView.RPC ("SendReady", RPCMode.Server);
	}
	[RPC]
	void SendReady ()
	{ }

	[RPC]
	public void UserJoin (string username, int type)
	{
		waitingLobby.AddUser (username, type);
	}

	[RPC]
	public void UserLeave (string username)
	{
		waitingLobby.RemoveUser (username);
	}




	// Game
}
