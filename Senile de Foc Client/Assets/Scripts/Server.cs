using UnityEngine;
using System.Collections;

//TODO switch from this dummy server 'interface'
public class Server : MonoBehaviour 
{
	// Login handling
	public static bool UsernameExists (string username)
	{
		return username == "stefan";
	}

	public static bool PasswordMatches (string username, string password)
	{
		return username == "stefan" && password == "a";
	}

	public static void CreateUser (string username, string password)
	{
		print ("created " + username + ", " + password);
	}

	public static void Login (string username, string password)
	{
		// I'm not sure this server interaction is needed
		print (username + " logged");
	}
}
