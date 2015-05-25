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
			NetworkStatus.Show ("No servers found", NetworkStatus.MessageType.failure);
		else 
			ConnectToFound ();
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
		
		NetworkStatus.Show ("Connecting to sserver", NetworkStatus.MessageType.working);
		Network.Connect (hostData [0]);
	}

}
