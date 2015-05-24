using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour 
{
	void OnGUI()
	{
		if (GUI.Button (new Rect (25, 25, 150, 30), "Start"))
			StartServer ();
	}

	void StartServer ()
	{
		Network.InitializeServer (NetworkConstants.MAX_PLAYERS, 
		                          NetworkConstants.PORT_NUMBER, 
		                          false);

		MasterServer.RegisterHost (NetworkConstants.GAME_TYPE,
		                           NetworkConstants.GAME_NAME);
	}

	void OnServerInitialized ()
	{
		Debug.Log ("server initialized");
	}

	void OnMasterServerEvent (MasterServerEvent MSEvent)
	{
		Debug.Log (MSEvent);
	}

	void OnPlayerConnected (NetworkPlayer player)
	{
		Debug.Log ("Player " + player + " connected from " + player.ipAddress + ":" + player.port);
	}

	void OnPlayerDisconnected (NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
}
