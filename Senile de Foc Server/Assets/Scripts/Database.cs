using UnityEngine;
using System.Collections;

public class Database : MonoBehaviour 
{
	string userCountKey = "user count";
	public int userCount
	{
		get { return PlayerPrefs.GetInt (userCountKey, 0); }
		set { PlayerPrefs.SetInt (userCountKey, value); }
	}

	string nameKey = "user";
	string passKey = "pass";

	public void Create (string name, string pass)
	{
		PlayerPrefs.SetString (nameKey + userCount, name);
		PlayerPrefs.SetString (passKey + userCount, pass);
		
		userCount++;
	}

	string NameNr (int index)
	{
		return PlayerPrefs.GetString (nameKey + index, "");
	}

	string PassNr (int index)
	{
		return PlayerPrefs.GetString (passKey + index, "");
	}

	int IndexOf (string name)
	{
		for (int i = 0; i < userCount; i++)
			if (NameNr (i) == name)
				return i;
		return -1;
	}
	
	public bool Exists (string name)
	{
		return IndexOf (name) != -1;
	}

	public bool Matches (string name, string pass)
	{
		int index = IndexOf (name);
		return pass == PassNr (index);
	}

	string PassOf (string name)
	{
		return PlayerPrefs.GetString (passKey + IndexOf (name), "");
	}

	public void PrintAllUsers ()
	{
		string users = "Users: \n";
		for (int i = 0; i < userCount; i++)
			users += NameNr (i) + "\t" + PassNr (i) + "\n";
		print (users);
	}

	public void ClearUsers ()
	{
		PlayerPrefs.DeleteAll ();
		print ("Deleted all users");
	}
}
