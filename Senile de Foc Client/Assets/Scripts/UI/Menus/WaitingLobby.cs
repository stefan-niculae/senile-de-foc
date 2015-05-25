using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaitingLobby : MonoBehaviour
{
	public GameObject lobbyInfoPrefab;
	public static List <PlayerInfo> currentPlayers;

	Transform container;
	static readonly float DISTANCE = 65f;

	
	void Awake ()
	{
		container = GameObject.Find ("Connected Players").transform;
		currentPlayers = new List <PlayerInfo> ();
	}

	public void BackToSelection ()
	{
		SplashMenus.currentStep = SplashMenus.Steps.selection;
	}

	public void PopulateList (List <PlayerInfo> playerInfos)
	{
		currentPlayers = playerInfos;

		foreach (Transform child in container)
			if (child != container)
				Destroy (child.gameObject);

		for (int i = 0; i < playerInfos.Count; i++) {
			Vector3 pos = container.localPosition;
			pos.y = i * DISTANCE * -1;

			GameObject shownPlayer = Instantiate (lobbyInfoPrefab) as GameObject;
			shownPlayer.GetComponent <LobbyInfo> ().SetValues (playerInfos [i]);
			shownPlayer.transform.SetParent (container, false);
			shownPlayer.transform.localPosition = pos;
		}

	}

	public void MakeThisReady ()
	{
		SplashServer.RegisterReady ();
	}
}
