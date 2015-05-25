using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class RequestsManager : MonoBehaviour
{
	Database database;
	NetworkView netView;
	Dictionary <NetworkPlayer, PlayerInfo> connectedPlayers;

	public Text logText;
	string log
	{
		get { return logText.text; }
		set { logText.text = value + "\n"; }
	}
	public Text playersText;
	string players
	{
		get { return playersText.text; }
		set { playersText.text = value; }
	}




	
	void Awake ()
	{
		database = GameObject.FindObjectOfType <Database> ();
		netView = GetComponent <NetworkView> ();
		connectedPlayers = new Dictionary<NetworkPlayer, PlayerInfo> ();
	}




	// Connected Players
	void OnPlayerConnected (NetworkPlayer player)
	{
		log += "Player " + player + " connected from " + player.ipAddress + ":" + player.port;
		connectedPlayers.Add (player, new PlayerInfo (player));

		UpdatePlayers ();
	}
	
	void OnPlayerDisconnected (NetworkPlayer p)
	{
		log += p.ipAddress + " disconnected";
		if (connectedPlayers.ContainsKey (p))
			connectedPlayers.Remove (p);

		Network.RemoveRPCs(p);

		UpdatePlayers ();
	}

	void UpdatePlayers ()
	{
		string str = "";

		List <PlayerInfo> playerInfos = connectedPlayers.Values.ToList ();
		foreach (PlayerInfo p in playerInfos)
			str += p + "\n";

		players = str;
		netView.RPC ("ReceivePlayerList", RPCMode.Others, NetworkUtils.ObjectToByteArray (playerInfos));
	}
	[RPC]
	void ReceivePlayerList (byte[] bytes)
	{ }





	// Login/Register
	[RPC]
	void RequestUsernameExistance (string username, NetworkMessageInfo info)
	{
		bool value = database.Exists (username);
		log += "Existance of " + username + " = " + value;
		netView.RPC ("ReceiveUsernameExistance", info.sender, value);
	}
	[RPC]
	void ReceiveUsernameExistance (bool value)
	{ }

	[RPC]
	void SendCreateUser (string name, string pass)
	{
		log += "Creating " + name + ", " + pass[0] + "...";
		database.Create (name, pass);
	}

	[RPC]
	void RequestPasswordMatch (string username, string password, NetworkMessageInfo info)
	{
		bool value = database.Matches (username, password);
		log += "Match " + username + " with " + password [0] + "... = " + value;
		netView.RPC ("ReceivePasswordMatch", info.sender, value);
	}
	[RPC]
	void ReceivePasswordMatch (bool value)
	{ }

	[RPC]
	public void SendLogin (string username, NetworkMessageInfo info)
	{
		log += username + " logged in";
		connectedPlayers [info.sender].name = username;
		UpdatePlayers ();
	}





	// Tank Select
	[RPC]
	void SendTankType (byte[] bytes, NetworkMessageInfo info)
	{
		TankType type = NetworkUtils.ByteArrayToObject (bytes) as TankType;
		connectedPlayers [info.sender].tankType = type;
		UpdatePlayers ();

		log += connectedPlayers [info.sender].name + " chose " + type.slotNr;
	}

	[RPC]
	void SendRates (byte[] bytes, NetworkMessageInfo info)
	{ 
		Rates rates = NetworkUtils.ByteArrayToObject (bytes) as Rates;
		connectedPlayers [info.sender].rates = rates;
		UpdatePlayers ();

		log += connectedPlayers [info.sender].name + " picked rates";
	}





	// Lobby
	[RPC]
	public void SendReady (NetworkMessageInfo info)
	{
		connectedPlayers [info.sender].ready = true;
		UpdatePlayers ();

		bool allReady = true;
		foreach (PlayerInfo player in connectedPlayers.Values)
			if (!player.ready) {
				allReady = false;
				break;
			}

		if (allReady) {
			netView.RPC ("ReceiveGameStart", RPCMode.Others);
			log += "Everyone ready => game start";
		}
	}




	
	// Logout
	[RPC]
	public void SendLogout (NetworkMessageInfo info)
	{
		PlayerInfo player = connectedPlayers [info.sender];
		log += player.name + " logged out";

		connectedPlayers [info.sender].Reset ();
		UpdatePlayers ();
	}





	// Splash to Game
	[RPC]
	void ReceiveGameStart ()
	{ }
}
