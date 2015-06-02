using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Database : MonoBehaviour 
{
	const string USER_COUNT_KEY = "user count";
	public int userCount
	{
		get { return PlayerPrefs.GetInt (USER_COUNT_KEY, 0); }
		set { PlayerPrefs.SetInt (USER_COUNT_KEY, value); }
	}

	const string NAME_KEY = "user";
	const string PASS_KEY = "pass";

	public Text registeredText;
	string registered
	{
		get { return registeredText.text; }
		set { registeredText.text = value; }
	}


	const string BEST_PLAYER_KEY = "best player";
	public string bestPlayer
	{
		get { return PlayerPrefs.GetString (BEST_PLAYER_KEY, " - NONE - "); }
		set { PlayerPrefs.SetString (BEST_PLAYER_KEY, value); }
	}

	const string MOST_KILLS_KEY = "most kills";
	public int mostKills
	{
		get { return PlayerPrefs.GetInt (MOST_KILLS_KEY, -1); }
		set { PlayerPrefs.SetInt (MOST_KILLS_KEY, value); }
	}



	void Start ()
	{
		registered = AllUsers ();
	}

	public void Create (string name, string pass)
	{
		PlayerPrefs.SetString (NAME_KEY + userCount, name);
		PlayerPrefs.SetString (PASS_KEY + userCount, pass);
		
		userCount++;
		registered = AllUsers ();
	}

	string NameNr (int index)
	{
		return PlayerPrefs.GetString (NAME_KEY + index, "");
	}

	string PassNr (int index)
	{
		return PlayerPrefs.GetString (PASS_KEY + index, "");
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
		return PlayerPrefs.GetString (PASS_KEY + IndexOf (name), "");
	}

	public string AllUsers ()
	{
		string users = "";
		for (int i = 0; i < userCount; i++)
			users += NameNr (i) + "\t" + PassNr (i) + "\n";
		return users;
	}

	public void ClearUsers ()
	{
		PlayerPrefs.DeleteAll ();
		registered = AllUsers ();
	}

	public void UpdateHighscore (string username, int kills)
	{
		if (kills > mostKills) {
			mostKills = kills;
			bestPlayer = username;

			string path = Application.dataPath;
			if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
				path += "/../../";
			}
			else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
				path += "/../";
			}

			path += "highscore.txt";
			print ("Created highscore file at " + path);
			File.WriteAllText (path,
			                   username + " killed " + kills + "\n");

		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.H))
			UpdateHighscore ("best", 7);
		if (Input.GetKeyDown (KeyCode.E)) {
			PlayerPrefs.DeleteKey (BEST_PLAYER_KEY);
			PlayerPrefs.DeleteKey (MOST_KILLS_KEY);
			print ("deleted highscore");
		}
	}
}
