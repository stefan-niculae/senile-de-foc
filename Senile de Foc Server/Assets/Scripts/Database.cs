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
	const string PASS_FIRST_LETTER_KEY = "pass first letter";
	const string PASS_HASH_CODE_KEY = "pass hash code";

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

	const string 
		KILLS_KEY = "kills",
		DEATHS_KEY = "deaths",
		ASSISTS_KEY = "assists",
		BARRELS_KEY = "barrels";
	int bestKills
	{
		get { return PlayerPrefs.GetInt (KILLS_KEY, 0); }
		set { PlayerPrefs.SetInt (KILLS_KEY, value); }
	}
	int bestDeaths
	{
		get { return PlayerPrefs.GetInt (DEATHS_KEY, 0); }
		set { PlayerPrefs.SetInt (DEATHS_KEY, value); }
	}
	int bestAssists
	{
		get { return PlayerPrefs.GetInt (ASSISTS_KEY, 0); }
		set { PlayerPrefs.SetInt (ASSISTS_KEY, value); }
	}
	int bestBarrels
	{
		get { return PlayerPrefs.GetInt (BARRELS_KEY, 0); }
		set { PlayerPrefs.SetInt (BARRELS_KEY, value); }
	}
	Stats bestStats
	{
		get { return new Stats (bestKills, bestDeaths, bestAssists, bestBarrels); }
		set 
		{
			bestKills 	= value.kills;
			bestDeaths 	= value.deaths;
			bestAssists = value.assists;
			bestBarrels = value.barrels;
		}
	}


	void Start ()
	{
		registered = AllUsers ();
	}

	public void Create (string name, char passFirst, int passHash)
	{
		PlayerPrefs.SetString (NAME_KEY + userCount, name);
		PlayerPrefs.SetString (PASS_FIRST_LETTER_KEY + userCount, passFirst.ToString ());
		PlayerPrefs.SetInt (PASS_HASH_CODE_KEY + userCount, passHash);
		
		userCount++;
		registered = AllUsers ();
	}

	string NameNr (int index)
	{
		return PlayerPrefs.GetString (NAME_KEY + index, "");
	}
	char PassFirstLetterNr (int index)
	{
		return PlayerPrefs.GetString (PASS_FIRST_LETTER_KEY + index, " ")[0];
	}
	int PassHashCodeNr (int index)
	{
		return PlayerPrefs.GetInt (PASS_HASH_CODE_KEY + index, -1);
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

	public bool Matches (string name, int hashCode)
	{
		int index = IndexOf (name);
		return hashCode == PassHashCodeNr (index);
	}

	public string AllUsers ()
	{
		string users = "";
		for (int i = 0; i < userCount; i++)
			users += string.Format ("{0} {1}\t\t {2}...\t {3}\n", i, NameNr (i), PassFirstLetterNr (i), PassHashCodeNr (i));
		return users;
	}

	public void ClearUsers ()
	{
		PlayerPrefs.DeleteAll ();
		registered = AllUsers ();
	}

	public void UpdateHighscore (string username, Stats stats)
	{
		if (stats.CompareTo (bestStats) == 1) {
			bestPlayer = username;

			string path = Application.dataPath;
			if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
				path += "/../../";
			}
			else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
				path += "/../";
			}

			path += "highscore.txt";
			Debug.LogWarning ("Created highscore file " + path);
			File.WriteAllText (path,
			                   username + ": " + stats + "\n");
		}
	}

//	void Update ()
//	{
//		if (Input.GetKeyDown (KeyCode.E)) {
//			PlayerPrefs.DeleteKey (BEST_PLAYER_KEY);
//			PlayerPrefs.DeleteKey (KILLS_KEY);
//			PlayerPrefs.DeleteKey (DEATHS_KEY);
//			PlayerPrefs.DeleteKey (ASSISTS_KEY);
//			PlayerPrefs.DeleteKey (BARRELS_KEY);
//
//			Debug.LogWarning ("Cleared highscore (file still exists)");
//		}
//	}

	const string LAST_TIME_LIMIT_KEY  = "last time limit";
	public int lastTimeLimit
	{
		get { return PlayerPrefs.GetInt (LAST_TIME_LIMIT_KEY, 10); }
		set { PlayerPrefs.SetInt (LAST_TIME_LIMIT_KEY, value); }
	}
	
	const string LAST_KILLS_LIMIT_KEY = "last kills limit";
	public int lastKillsLimit
	{
		get { return PlayerPrefs.GetInt (LAST_KILLS_LIMIT_KEY, 10); }
		set { PlayerPrefs.SetInt (LAST_KILLS_LIMIT_KEY, value); }
	}
}
