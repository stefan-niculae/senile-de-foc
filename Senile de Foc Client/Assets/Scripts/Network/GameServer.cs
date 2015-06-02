using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameServer : Singleton<GameServer> 
{
	public static NetworkView netView;
	public static PlayerInfo selfInfo;
	public List<PlayerInfo> connectedPlayers;
	public Dictionary<int, Damagable> damageables;
	public Dictionary<int, TankInfo> orderNrToTankInfo;
	// TODO refactor lists and dictionaries to be used more efficiently

	void Awake ()
	{
		netView = Instance.GetComponent <NetworkView> ();
		SelfInfo (info => {
			selfInfo = info;
			NetworkStatus.Show ("Received self info, waiting for others", NetworkStatus.MessageType.working);
		});
		connectedPlayers = new List<PlayerInfo> ();
		if (damageables == null)
			damageables = new Dictionary<int, Damagable> ();
		orderNrToTankInfo = new Dictionary<int, TankInfo> ();
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
		Scoreboard.Instance.PopulateList (connectedPlayers);

		NetworkStatus.Show ("Updated player list", NetworkStatus.MessageType.success);
	}

	[RPC]
	void ReceiveMatchStart ()
	{
		NetworkStatus.Show ("Everyone connected, match starts", NetworkStatus.MessageType.success);
		((IngameUIManager)IngameUIManager.Instance).state = IngameUIManager.State.playing;
		MarkerManager.Instance.Spawn ();

	}

	public void AnnounceTakingDamage (float damage, int source, int destination)
	{
		GameServer.netView.RPC ("ReceiveDamageAnnouncement", RPCMode.Others, damage, source, destination);
	}
	[RPC]
	public void ReceiveDamageAnnouncement (float damage, int source, int destination)
	{
		damageables [destination].TakeDamage (damage, source, announce: false);
	}


	public void SendStatsUpdate (int orderNumber)
	{
		var stats = orderNrToTankInfo [orderNumber].playerInfo.stats;
		netView.RPC ("ReceiveStatsUpdate", RPCMode.All, orderNumber, NetworkUtils.ObjectToByteArray (stats));
	}
	[RPC]
	void ReceiveStatsUpdate (int orderNumber, byte[] statsBytes)
	{
		var stats = NetworkUtils.ByteArrayToObject (statsBytes) as Stats;
		orderNrToTankInfo [orderNumber].playerInfo.stats = stats;

		foreach (var p in connectedPlayers)
			if (p.orderNumber == orderNumber)
				p.stats = stats;
		Scoreboard.Instance.PopulateList (connectedPlayers);
	}
}
