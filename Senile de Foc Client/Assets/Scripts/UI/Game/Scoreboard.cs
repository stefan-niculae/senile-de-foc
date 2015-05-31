using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : Singleton<Scoreboard>
{
	public GameObject ingameInfoPrefab;

	Transform container;
	static readonly float DISTANCE = 50f;

	Vector3 shownPos;
	public bool isShown;


	void Awake ()
	{
		shownPos = transform.localPosition;
		container = GameObject.Find ("Players Info").transform;
	}

	void Update ()
	{
		isShown = Input.GetKey (KeyCode.Tab);

		transform.localPosition = (isShown ? shownPos : Constants.HIDDEN);
	}

	public void PopulateList (List <PlayerInfo> playerInfos)
	{
		foreach (Transform child in container)
			if (child != container)
				Destroy (child.gameObject);

		for (int i = 0; i < playerInfos.Count; i++) {
			Vector3 pos = container.localPosition;
			pos.y = i * DISTANCE * -1;

			GameObject shownPlayer = Instantiate (ingameInfoPrefab) as GameObject;

			shownPlayer.GetComponent <IngameInfo> ().SetValues (playerInfos [i]);
			shownPlayer.transform.localPosition = pos;
			shownPlayer.transform.SetParent (container, false);
		}
	}
}
