using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameServer : Singleton<GameServer> 
{
	public static NetworkView netView;
	public static PlayerInfo selfInfo;
	Scoreboard scoreboard;
	public List<PlayerInfo> connectedPlayers;
	public Dictionary<int, Damagable> damageables;

	void Awake ()
	{
		netView = Instance.GetComponent <NetworkView> ();
		scoreboard = GameObject.Find ("Scoreboard").GetComponent <Scoreboard> ();
		SelfInfo (info => {
			selfInfo = info;
			NetworkStatus.Show ("Received self info, waiting for others", NetworkStatus.MessageType.working);
		});
		connectedPlayers = new List<PlayerInfo> ();
		if (damageables == null)
			damageables = new Dictionary<int, Damagable> ();
	}

	static Action<PlayerInfo> onSelfInfoReceival;
	public static void SelfInfo (Action<PlayerInfo> onReceival)
	{
		onSelfInfoReceival = onReceival;
		netView.RPC ("RequestInfo", RPCMode.Server);
		NetworkStatus.Show ("Requesting self info", NetworkStatus.MessageType.working);
	}
	[RPC]
	void RequestInfo ()
	{ }
	[RPC]
	public void ReceiveInfo (byte[] bytes)
	{
		PlayerInfo info = NetworkUtils.ByteArrayToObject (bytes) as PlayerInfo;
		onSelfInfoReceival (info);
		NetworkStatus.Show ("Connected as " + info.name, NetworkStatus.MessageType.success);
	}
	
	[RPC]
	void RequestPlayerList ()
	{ }
	[RPC]
	void ReceivePlayerList (byte[] received)
	{
		// The received byte array represents the serialization of a list containing player infos
		connectedPlayers = NetworkUtils.ByteArrayToObject (received) as List<PlayerInfo>;
		scoreboard.PopulateList (connectedPlayers);

		NetworkStatus.Show ("Updated player list", NetworkStatus.MessageType.success);
	}

	[RPC]
	void ReceiveMatchStart ()
	{
		NetworkStatus.Show ("Everyone connected, match starts", NetworkStatus.MessageType.success);
		UIManager.Instance.SetVisibility (true);
		MarkerManager.Instance.Spawn ();
	}

	public void SendHealthUpdate (int networkID, float amount)
	{
		GameServer.netView.RPC ("ReceiveHealthUpdate", RPCMode.Others, networkID, amount);
	}

	[RPC]
	public void ReceiveHealthUpdate (int networkID, float amount)
	{
		damageables [networkID].amount = amount;
	}
}
