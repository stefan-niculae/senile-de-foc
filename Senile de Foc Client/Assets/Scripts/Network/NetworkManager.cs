using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
	HostData[] hostData;
	static readonly float SEARCH_DURATION = 3f;

	void Start ()
	{
		StartConnecting ();
	}

	void StartConnecting ()
	{
	  	StartCoroutine (GetHostList ());
	}

	IEnumerator GetHostList ()
	{
		NetworkStatus.Show ("Looking for servers", NetworkStatus.MessageType.working);
		MasterServer.RequestHostList (NetworkConstants.GAME_TYPE);

		float endSearchTime = Time.time + SEARCH_DURATION;
		do {
			hostData = MasterServer.PollHostList ();
			yield return new WaitForEndOfFrame ();
		} while (hostData.Length == 0 && Time.time < endSearchTime);

		if (hostData.Length == 0) 
			ConnectToLocal ();
		else 
			ConnectToFound ();
	}

	void ConnectToLocal ()
	{
		NetworkStatus.Show ("Master server offline, connecting to local server", NetworkStatus.MessageType.working);
		Network.Connect (NetworkConstants.LOCAL_SERVER_IP,
		                 NetworkConstants.PORT_NUMBER);
	}

	void OnConnectedToServer ()
	{
		NetworkStatus.Show ("Connected to server", NetworkStatus.MessageType.success);
	}

	void OnDisconnectedFromServer (NetworkDisconnection info)
	{
		NetworkStatus.Show ("Disconnected from the server " + info, NetworkStatus.MessageType.failure);
	}

	void ConnectToFound ()
	{
		if (hostData.Length > 1)
			Debug.Log ("More than one servers found");
		
		NetworkStatus.Show ("Connecting to server", NetworkStatus.MessageType.working);
		Network.Connect (hostData [0]);
	}

}
