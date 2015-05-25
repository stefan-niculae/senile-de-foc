using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaitingLobby : MonoBehaviour
{
	public Sprite readyCheckmark;
	public GameObject playerInfoPrefab;

	Transform container;
	static readonly float DISTANCE = 65f;

	Image[] bodies;
	Image[] barrels;

	List<GameObject> connectedPlayers;
	
	
	void Awake ()
	{
		container = GameObject.Find ("Connected Players").transform;
		connectedPlayers = new List<GameObject> ();

		bodies = new Image[5];
		barrels = new Image[5];

		bodies [0] = GameObject.Find ("Calm Tank Body").GetComponent <Image> ();
		barrels [0] = GameObject.Find ("Calm Tank Barrel").GetComponent <Image> ();

		bodies [1] = GameObject.Find ("Heavy Tank Body").GetComponent <Image> ();
		barrels [1] = GameObject.Find ("Heavy Tank Barrel").GetComponent <Image> ();

		bodies [2] = GameObject.Find ("Angry Tank Body").GetComponent <Image> ();
		barrels [2] = GameObject.Find ("Angry Tank Barrel").GetComponent <Image> ();

		bodies [3] = GameObject.Find ("Sneaky Tank Body").GetComponent <Image> ();
		barrels [3] = GameObject.Find ("Sneaky Tank Barrel").GetComponent <Image> ();
		
		bodies [4] = GameObject.Find ("Custom Tank Body").GetComponent <Image> ();
		barrels [4] = GameObject.Find ("Custom Tank Barrel").GetComponent <Image> ();
	}

	public void BackToSelection ()
	{
		SplashMenus.currentStep = SplashMenus.Steps.selection;
	}

	// Usernames are unique so it's ok to use them as a primary key
	// also there can be a maximum of 4 players so it's ok to search by string
	public void AddUser (string username, int tankType)
	{
		GameObject player = Instantiate (playerInfoPrefab) as GameObject;
		player.GetComponent <PlayerInLobby> ().Init (username, bodies [tankType], barrels [tankType]);

		player.transform.SetParent (container, false);
		player.transform.localScale = Vector3.one;

		connectedPlayers.Add (player);
		ArrangeList ();
	}

	public void SetUserReady (string username)
	{
		GameObject toChange = null;
		foreach (var player in connectedPlayers)
		if (player.GetComponent <PlayerInLobby> ().username == username) {
			toChange = player;
			break;
		}
		toChange.GetComponent <PlayerInLobby> ().MakeReady (readyCheckmark);
	}
	
	public void RemoveUser (string username)
	{
		GameObject toRemove = null;
		foreach (var player in connectedPlayers)
			if (player.GetComponent <PlayerInLobby> ().username == username) {
				toRemove = player;
				break;
			}
		connectedPlayers.Remove (toRemove);
		Destroy (toRemove);

		ArrangeList ();
	}

	void ArrangeList ()
	{
		for (int i = 0; i < connectedPlayers.Count; i++) {
			Vector3 pos = container.localPosition;
			pos.y = i * DISTANCE * -1;
			connectedPlayers [i].transform.localPosition = pos;
		}
	}


	public void MakeThisReady ()
	{
		Server.RegisterReady ();
	}
}
