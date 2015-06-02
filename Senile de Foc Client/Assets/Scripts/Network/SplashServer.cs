using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SplashServer : Singleton<SplashServer>
{
	static NetworkView netView;
	TankSelection tankSelection;
	WaitingLobby waitingLobby;

	void Awake ()
	{
		netView = Instance.GetComponent <NetworkView> ();

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
		NetworkStatus.Show ("Checking if password is correct", NetworkStatus.MessageType.working);
	}
	[RPC]
	void RequestPasswordMatch (string username, string password)
	{ }
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
		netView.RPC ("SendLogout", RPCMode.Server);
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
	void ReceivePlayerList (byte[] received)
	{
		// The received byte array represents the serialization of a list containing player infos
		List<PlayerInfo> playerInfos = NetworkUtils.ByteArrayToObject (received) as List<PlayerInfo>;
		waitingLobby.PopulateList (playerInfos);

		for (int i = 0; i < 4; i++)
			tankSelection.SetAvailability (i, true);
		foreach (PlayerInfo info in playerInfos) 
			if (info.tankType.slotNr != NetworkConstants.NOT_SET)
				tankSelection.SetAvailability (info.tankType.slotNr, false);


		NetworkStatus.Show ("Received new player list", NetworkStatus.MessageType.success);
	}

	[RPC]
	public void ReceiveGameStart ()
	{
		LoadingManager.StartLoading ("Battlefield");
	}
}
