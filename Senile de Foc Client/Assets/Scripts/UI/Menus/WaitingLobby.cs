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

	Sprite[] bodies;
	Sprite[] barrels;

	List<GameObject> connectedPlayers;
	
	
	string dummyusername = "username";
	void Awake ()
	{Debug.Log ("Server simulation: c, d, r to connect/disconnect/ready a dummy user");

		container = GameObject.Find ("Connected Players").transform;
		connectedPlayers = new List<GameObject> ();

		bodies = new Sprite[5];
		barrels = new Sprite[5];

		bodies [0] = Utils.childWithName (GameObject.Find ("Calm Tank Preview").transform, "Body").GetComponent <Image> ().sprite;
		barrels [0] = Utils.childWithName (GameObject.Find ("Calm Tank Preview").transform, "Barrel").GetComponent <Image> ().sprite;

		bodies [1] = Utils.childWithName (GameObject.Find ("Heavy Tank Preview").transform, "Body").GetComponent <Image> ().sprite;
		barrels [1] = Utils.childWithName (GameObject.Find ("Heavy Tank Preview").transform, "Barrel").GetComponent <Image> ().sprite;

		bodies [2] = Utils.childWithName (GameObject.Find ("Angry Tank Preview").transform, "Body").GetComponent <Image> ().sprite;
		barrels [2] = Utils.childWithName (GameObject.Find ("Angry Tank Preview").transform, "Barrel").GetComponent <Image> ().sprite;

		bodies [3] = Utils.childWithName (GameObject.Find ("Sneaky Tank Preview").transform, "Body").GetComponent <Image> ().sprite;
		barrels [3] = Utils.childWithName (GameObject.Find ("Sneaky Tank Preview").transform, "Barrel").GetComponent <Image> ().sprite;

		bodies [4] = Utils.childWithName (GameObject.Find ("Custom Tank Preview").transform, "Body").GetComponent <Image> ().sprite;
		barrels [4] = Utils.childWithName (GameObject.Find ("Custom Tank Preview").transform, "Barrel").GetComponent <Image> ().sprite;
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
		player.GetComponent <PlayerInfo> ().Init (username, bodies [tankType], barrels [tankType]);

		player.transform.SetParent (container, false);
		player.transform.localScale = Vector3.one;

		connectedPlayers.Add (player);
		ArrangeList ();
	}

	public void SetUserReady (string username)
	{
		GameObject toChange = null;
		foreach (var player in connectedPlayers)
		if (player.GetComponent <PlayerInfo> ().username == username) {
			toChange = player;
			break;
		}
		toChange.GetComponent <PlayerInfo> ().MakeReady (readyCheckmark);
	}
	
	public void RemoveUser (string username)
	{
		GameObject toRemove = null;
		foreach (var player in connectedPlayers)
			if (player.GetComponent <PlayerInfo> ().username == username) {
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
		Server.RegisterReady (SplashMenus.currentUsername);
		//TODO again, make sure to disable this to avoid duplication
		SetUserReady (SplashMenus.currentUsername);
	}
	
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.C))
			AddUser (dummyusername, Random.Range (0, 4));
		if (Input.GetKeyDown (KeyCode.D))
			RemoveUser (dummyusername);
		if (Input.GetKeyDown (KeyCode.R))
			SetUserReady (dummyusername);
	}
}
