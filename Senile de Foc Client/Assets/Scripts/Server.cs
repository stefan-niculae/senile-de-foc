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

	public static bool PasswordMatches (string password)
	{
		return LoginForm.usernameField.text == "stefan" && password == "a";
	}
}
