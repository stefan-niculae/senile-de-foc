using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : Singleton<Scoreboard>
{
	public GameObject ingameInfoPrefab;

	Transform container;
	static readonly float DISTANCE = 50f;

	public bool isShown;

	Dictionary <int, IngameInfo> orderNumToIngameInfo;

	void Awake ()
	{
		container = GameObject.Find ("Players Info").transform;
		orderNumToIngameInfo = new Dictionary<int, IngameInfo> ();
	}

	void Update ()
	{
		isShown = Input.GetKey (KeyCode.Tab);

		transform.localPosition 				 = new Vector3 ( isShown ? 0 : Constants.HIDDEN.x, 					transform.localPosition.y, 0);
		UIManager.Instance.respawn.localPosition = new Vector3 (!isShown ? 0 : Constants.HIDDEN.x, UIManager.Instance.respawn.localPosition.y, 0);

		if (isShown)
			UIManager.Instance.ClearPopup ();
	}

	public void PopulateList (List <PlayerInfo> playerInfos)
	{
		foreach (Transform child in container)
			if (child != container)
				Destroy (child.gameObject);
		orderNumToIngameInfo.Clear ();
		
		playerInfos.Sort ();
		for (int i = 0; i < playerInfos.Count; i++) {
			Vector3 pos = container.localPosition;
			pos.y = i * DISTANCE * -1;

			GameObject shownPlayer = Instantiate (ingameInfoPrefab) as GameObject;
			orderNumToIngameInfo [playerInfos [i].orderNumber] = shownPlayer.GetComponent <IngameInfo> ();

			shownPlayer.GetComponent <IngameInfo> ().SetValues (playerInfos [i]);
			shownPlayer.transform.localPosition = pos;
			shownPlayer.transform.SetParent (container, false);
		}
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
