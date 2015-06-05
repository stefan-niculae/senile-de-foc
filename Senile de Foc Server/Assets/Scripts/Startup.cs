using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Startup : MonoBehaviour 
{
	public Text statusText;
	public Text logText;

	string status
	{
		get { return statusText.text; }
		set { statusText.text = "Status: " + value; }
	}
	string log
	{
		get { return logText.text; }
		set { logText.text = value; }
	}


	void Start ()
	{
		status = "Off";
		StartServer ();
	}

	void StartServer ()
	{
		status = "Initializing";
		Network.maxConnections = NetworkConstants.MAX_PLAYERS;
		Network.InitializeServer (NetworkConstants.MAX_PLAYERS, 
		                          NetworkConstants.PORT_NUMBER, 
		                          false);

		status = "Registering host";
		MasterServer.RegisterHost (NetworkConstants.GAME_TYPE,
		                           NetworkConstants.GAME_NAME);
	}

	void OnServerInitialized ()
	{
		status = "Initialized";
	}

	void OnMasterServerEvent (MasterServerEvent MSEvent)
	{
		log += MSEvent + "\n";
	}
}
