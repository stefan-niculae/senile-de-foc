using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WaitingLobby : MonoBehaviour
{
	public GameObject lobbyInfoPrefab;
	public static List <PlayerInfo> currentPlayers;

	Transform container;
	const float DISTANCE = 65f;

	public IncremetableValue time;
	public IncremetableValue kills;
	
	void Awake ()
	{
		container = GameObject.Find ("Connected Players").transform;
		currentPlayers = new List <PlayerInfo> ();

		time  = new IncremetableValue ("Time",  new int[] { 1, 5, 10, 15, 20, 30  }, "'", 	SplashServer.SendTimeLimit);
		kills = new IncremetableValue ("Kills", new int[] { 1, 10, 25, 50, 100 }, "",  		SplashServer.SendKillsLimit);
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

	// Stupid button systems
	public void KM () { kills--; }
	public void KP () { kills++; }
	public void TM () { time --; }
	public void TP () { time ++; }
}

public class IncremetableValue
{
	Button minus, plus;
	Text text;
	string additionalDisplay;
	Action<int> onChange;

	int[] thresholds;

	int index = 0;
	int Index
	{
		get { return index; }
		set
		{
			index = value;
			minus.interactable 	= (index > 0);
			plus.interactable  	= (index < thresholds.Length - 1);

			text.text = Amount + additionalDisplay;
		}
	}

	public int Amount
	{
		get 
		{
			return thresholds [Index];
		}
		set
		{
			// Will fail exception if the threhsolds array does not contain the value
			Index = Array.IndexOf (thresholds, value);
		}
	}


	public IncremetableValue (string hierarchyName, int[] thresholds, string additionalDisplay, Action<int> onChange)
	{
		text 	= GameObject.Find (hierarchyName + " Amount")	.GetComponent <Text> ();
		minus 	= GameObject.Find (hierarchyName + "-")			.GetComponent <Button> ();
		plus 	= GameObject.Find (hierarchyName + "+")			.GetComponent <Button> ();

		this.thresholds = thresholds;

		this.additionalDisplay = additionalDisplay;
		this.onChange = onChange;
	}

	public static IncremetableValue operator++ (IncremetableValue obj)
	{
		obj.Index++;
		obj.onChange (obj.Amount);
		return obj;
	}
	public static IncremetableValue operator-- (IncremetableValue obj)
	{
		obj.Index--;
		obj.onChange (obj.Amount);
		return obj;
	}
}