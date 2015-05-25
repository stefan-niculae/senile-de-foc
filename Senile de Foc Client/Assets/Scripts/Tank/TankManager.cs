using UnityEngine;
using System.Collections;

public class TankManager : Singleton<TankManager> 
{
	public GameObject tankPrefab;

	public void Spawn (PlayerInfo info)
	{
		Network.Instantiate (tankPrefab, Vector3.zero, Quaternion.identity, 0);
	}
}
