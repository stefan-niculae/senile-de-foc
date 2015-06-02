using UnityEngine;
using System.Collections;

public class MatchOver : Singleton<MatchOver> 
{
	public void YankPlayers ()
	{
		var players = GameObject.Find ("Players Table").transform;
		players.SetParent (transform, false);
		players.SetSiblingIndex (1);
	}

	public void GoToSplash ()
	{
		LoadingManager.StartLoading ("Splash");
	}
}
