using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HittersDisplay : Singleton<HittersDisplay> 
{
	public GameObject hitterInfoPrefab;

	Transform container;
	const float DISTANCE = 50f;

	void Awake ()
	{
		container = GameObject.Find ("Hitters Info").transform;
	}

	/**
	 * hitters contains order numbers as keys
	 * and damage done as values
	 * it is sorted descending by damage done descending
	 */
	public void PopulateList (List <KeyValuePair <int, float>> hitters)
	{
		var n = hitters.Count;
		PlayerInfo[]info 	= new PlayerInfo[n];
		float[] 	dmgPer 	= new float[n];

		float totalDmg = 0;
		hitters.ForEach (h => totalDmg += h.Value);
		for (int i = 0; i < n; i++) {
			info [i] = GameServer.Instance.orderNrToTankInfo [hitters [i].Key].playerInfo;
			dmgPer [i] = hitters [i].Value / totalDmg * 100;
		}

		foreach (Transform child in container)
			if (child != container)
				Destroy (child.gameObject);


		for (int i = 0; i < n; i++) {
			Vector3 pos = container.localPosition;
			pos.y = i * DISTANCE * -1;
			GameObject shownPlayer = Instantiate (hitterInfoPrefab) as GameObject;
			shownPlayer.transform.localPosition = pos;
			shownPlayer.transform.SetParent (container, false);

			var hitterInfo = shownPlayer.GetComponent <HitterInfo> ();
			hitterInfo.SetValues (info [i]);
			hitterInfo.SetDamagePer (dmgPer [i]);
		}
	}
}
