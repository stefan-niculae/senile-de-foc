using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour 
{
	public Dictionary <NetworkPlayer, Player> statsOf;
	public static PlayerManager instance;

	void Awake ()
	{
		statsOf = new Dictionary<NetworkPlayer, Player> ();
		instance = this;
	}

}
