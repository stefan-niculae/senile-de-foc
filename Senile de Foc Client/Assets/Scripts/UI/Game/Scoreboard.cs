using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Scoreboard : Singleton<Scoreboard>
{
	public GameObject ingameInfoPrefab;

	Transform container;
	const float DISTANCE = 50f;

	public bool isShown;
	[HideInInspector] public Transform respawn;

	[HideInInspector] public TankInfo localTank;

	Text killcountText;
	int matchKillcount;
	int MatchKillCount
	{
		get 
		{
			return matchKillcount;
		}
		set 
		{
			matchKillcount = value;
			killcountText.text = matchKillcount + " / " + GameServer.Instance.killsLimit;
		}
	}

	Dictionary <int, IngameInfo> orderNumToIngameInfo;

	void Awake ()
	{
		container = GameObject.Find ("Players Info").transform;
		orderNumToIngameInfo = new Dictionary<int, IngameInfo> ();

		killcountText = GameObject.Find ("Match Killcount").GetComponent <Text> ();
	}

	void Start ()
	{
		// Clear the dummy player infos created in the editor
		foreach (Transform child in container)
			if (child != container)
				Destroy (child.gameObject);
	}

	void Update ()
	{
		isShown = Input.GetKey (KeyCode.Tab);

		transform.localPosition	 = new Vector3 ( isShown ? 0 : Constants.HIDDEN.x, transform.localPosition.y, 0);
		if (respawn != null)
			respawn.localPosition= new Vector3 (!isShown ? 0 : Constants.HIDDEN.x, respawn.localPosition.y, 0);

		if (isShown) 
			IngameUIManager.DerivedInstance.ClearPopup ();
		
	}

	public void PopulateList (List <PlayerInfo> playerInfos)
	{
		// Erase everything and start from scratch when someone disconnects
		if (orderNumToIngameInfo.Count != playerInfos.Count && IngameUIManager.DerivedInstance.state != IngameUIManager.State.matchOver) {
			orderNumToIngameInfo.Clear ();
			foreach (Transform child in container)
				if (child != container)
					Destroy (child.gameObject);
		}
		
		// Sort descending
		playerInfos.Sort ((a, b) => -a.CompareTo (b));
		for (int i = 0; i < playerInfos.Count; i++) {

			if (!orderNumToIngameInfo.ContainsKey (playerInfos [i].orderNumber)) {
				GameObject shownPlayer = Instantiate (ingameInfoPrefab) as GameObject;
				shownPlayer.transform.SetParent (container, false);
				orderNumToIngameInfo [playerInfos [i].orderNumber] = shownPlayer.GetComponent <IngameInfo> ();
			}

			var ingameInfo = orderNumToIngameInfo [playerInfos [i].orderNumber];
			ingameInfo.SetValues (playerInfos [i]);
			ingameInfo.SetRank (i + 1); // first rank is 1, not zero
			ingameInfo.transform.position = container.position;
			ingameInfo.transform.localPosition = new Vector3 (0, i * DISTANCE * -1 + 100, 0); // I don't know where that +100 comes from...
		}

		UpdateKillcount ();
	
		// Before local tank instantiation
		if (localTank != null)
			localTank.ShowStatsRecap ();
	}

	public void UpdateKillcount ()
	{
		int kills = 0;
		foreach (var p in orderNumToIngameInfo.Values)
			kills += int.Parse (p.kills.text);
		MatchKillCount = kills;
	}

	public void StartCountdownFor (int orderNumber, float time)
	{
		IngameInfo info = orderNumToIngameInfo [orderNumber];
		info.username.color = References.Instance.deadColor;
		info.respawn.StartIt (time, ResetColors);
	}

	void ResetColors ()
	{
		foreach (var info in orderNumToIngameInfo.Values)
			if (info.respawn.val <= 0)
				info.username.color = References.Instance.aliveColor;
	}
}
