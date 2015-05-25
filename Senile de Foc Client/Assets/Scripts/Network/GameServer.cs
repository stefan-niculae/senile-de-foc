using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameServer : Singleton<GameServer> 
{
	static NetworkView netView;
	public string selfName;
	Scoreboard scoreboard;

	void Awake ()
	{
		netView = Instance.GetComponent <NetworkView> ();
		scoreboard = GameObject.Find ("Scoreboard").GetComponent <Scoreboard> ();
		SelfUsername (name => {
			selfName = name;
			netView.RPC ("RequestPlayerList", RPCMode.Server);

			onSelfInfoReceival = playerInfo => {
				TankManager.Instance.Spawn (playerInfo);
				onSelfInfoReceival = null;
			};

		});
	}

	static Action<string> onUsernameReceival;
	public static void SelfUsername (Action<string> onReceival)
	{
		onUsernameReceival = onReceival;
		netView.RPC ("RequestUsername", RPCMode.Server);
	}
	[RPC]
	void RequestUsername ()
	{ }
	[RPC]
	public void ReceiveUsername (string username)
	{
		onUsernameReceival (username);
	}

	static Action<PlayerInfo> onSelfInfoReceival = null;
	[RPC]
	void RequestPlayerList ()
	{ }
	[RPC]
	void ReceivePlayerList (byte[] received)
	{
		// The received byte array represents the serialization of a list containing player infos
		List<PlayerInfo> playerInfos = NetworkUtils.ByteArrayToObject (received) as List<PlayerInfo>;
		scoreboard.PopulateList (playerInfos);

		if (onSelfInfoReceival != null)
			foreach (var player in playerInfos)
				if (player.name == selfName) {
					onSelfInfoReceival (player);
					break;
				}

	}
}
