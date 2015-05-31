using UnityEngine;
using System.Collections;

public class MarkerManager : Singleton<MarkerManager> 
{
	public GameObject markerPrefab;

	// TODO maybe switch this to directly instantiate it
	public void Spawn ()
	{
		Transform point = GameWorld.spawnPoints [GameServer.selfInfo.orderNumber];
		Network.Instantiate (markerPrefab, point.position, point.rotation, 0);
	}

	public PlayerInfo PlayerFromSpawnPos (Vector3 pos)
	{
		int index = -1;
		for (int i = 0; i < GameWorld.spawnPoints.Length; i++)
			if (pos == GameWorld.spawnPoints [i].position) {
				index = i;
				break;
			}

		foreach (var p in GameServer.Instance.connectedPlayers)
			if (p.orderNumber == index)
				return p;
		return null;
	}
}
