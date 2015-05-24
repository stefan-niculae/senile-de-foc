using UnityEngine;
using System.Collections;

public class LoginHandling : MonoBehaviour
{
	Database database;
	NetworkView netView;

	void Awake ()
	{
		database = GameObject.FindObjectOfType <Database> ();
		netView = GetComponent <NetworkView> ();
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (200, 25, 100, 25), "Print Users"))
			database.PrintAllUsers ();
		if (GUI.Button (new Rect (200, 50, 100, 25), "Clear Users"))
			database.ClearUsers ();
	}

	[RPC]
	void RequestUsernameExistance (string username, NetworkMessageInfo info)
	{
		bool value = database.Exists (username);
		print ("\tgot request of existance for " + username + ", sending back " + value);
		netView.RPC ("ReceiveUsernameExistance", info.sender, value);
	}
	[RPC]
	void ReceiveUsernameExistance (bool value)
	{ }

	[RPC]
	void SendCreateUser (string name, string pass)
	{
		print ("\tcreating " + name + ", " + pass);
		database.Create (name, pass);
	}

	[RPC]
	void RequestPasswordMatch (string username, string password, NetworkMessageInfo info)
	{
		bool value = database.Matches (username, password);
		print ("\tgot request for match of " + username + ", " + password + ", sending back " + value);
		netView.RPC ("ReceivePasswordMatch", info.sender, value);
	}
	[RPC]
	void ReceivePasswordMatch (bool value)
	{ }

	[RPC]
	public void SendLogin (string username, NetworkMessageInfo info)
	{
		PlayerManager.instance.statsOf.Add (info.sender, new Player (username));
	}

	[RPC]
	void SendSelectTank (int slotNr, int body, int barrel, int primary, int secondary,
	                     int damage, int rate, int armor,  int speed, 
	                     NetworkMessageInfo info)
	{
//		netView.RPC ("ReceiveDisableOption", RPCMode.OthersBuffered, slotNr);

		Player player = PlayerManager.instance.statsOf [info.sender];

		player.body = body;
		player.barrel = barrel;
		player.primary = primary;
		player.secondary = secondary;
		player.damage = damage;
		player.rate = rate;
		player.armor = armor;
		player.speed = speed;

		PlayerManager.instance.statsOf [info.sender] = player;
	}
//
//	[RPC]
//	void ReceiveDisableOption (int slotNr)
//	{
//		print ("\tdisabled slot nr " + slotNr);
//	}
}
