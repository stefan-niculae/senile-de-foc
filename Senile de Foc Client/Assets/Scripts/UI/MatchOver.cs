using UnityEngine;
using System.Collections;

public class MatchOver : Singleton<MatchOver> 
{
	public void YankPlayers ()
	{
		var players = GameObject.Find ("Players Table").transform;
		var headers = GameObject.Find ("Headers").transform;

		players.SetParent (transform, false);
		players.SetSiblingIndex (1);

		headers.SetParent (transform, false);
		headers.SetSiblingIndex (2);
	}

	public void GoToSplash ()
	{
		LoadingManager.StartLoading ("Splash");
	}
}
