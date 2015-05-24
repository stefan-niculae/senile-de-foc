using UnityEngine;
using System.Collections;

public class Server : Singleton<Server>
{
	static string u1, p1;

	static NetworkView netView;

	void Awake ()
	{
		netView = Instance.GetComponent <NetworkView> ();
	}

	// Login/Register
	public static bool UsernameExists (string username)
	{
		return username == "s";
	}

	public static bool PasswordMatches (string username, string password)
	{
		return (username == "s" && password == "a") || (username == u1 && password == p1);
	}

	public static void CreateUser (string username, string password)
	{
		print ("created " + username + ", " + password);
		u1 = username; p1 = password;
	}
	
	public static void Login (string username, string password)
	{
		print ("a");
		netView.RPC ("DoLogin", RPCMode.Server, username, password);
		print ("b");
	}

	[RPC]
	void DoLogin (string username, string password)
	{
		NetworkStatus.Show ("Logging in", NetworkStatus.MessageType.working);
	}

	public static void Logout ()
	{
		//again, not sure this is needed
		print ("logged out");
	}


	// Tank Select
	public static void SelectTank (int number)
	{
		// TODO signal to others to disable this selection
		// TODO also when a new player logs in, send the list of disabled tanks
		print ("picked " + number);
	}

	public static void RegisterCustom (int cannon, int special, int damage, int rate, int armor, int speed)
	{
		print ("registered custom tank");
	}


	// Lobby
	public static void RegisterReady (string username)
	{
		print (username + " is ready");
	}





	// Game
}
