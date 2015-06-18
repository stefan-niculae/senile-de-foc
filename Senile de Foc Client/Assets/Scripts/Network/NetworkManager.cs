using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour 
{
	HostData[] hostData;
	const float SEARCH_DURATION = 3f;
	const float RECONNECT_INTERVAL = 10f;

	Button loginButton;
	bool _connected;
	bool connected
	{
		get { return _connected; }
		set 
		{
			_connected = value;
			loginButton.interactable = value;
		}
	}

	void Awake ()
	{
		loginButton = GameObject.Find ("Button Login").GetComponent <Button> ();
		connected = false;
	}

	void Start ()
	{
		InvokeRepeating ("StartConnecting", 0, RECONNECT_INTERVAL);
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
		NetworkStatus.Show ("Master server offline, trying local (" + LobbyNetwork.Instance.prefferedIP + ")", NetworkStatus.MessageType.working);
		Network.Connect (LobbyNetwork.Instance.prefferedIP,
						 LobbyNetwork.Instance.prefferedPort);
	}

	void OnConnectedToServer ()
	{
		CancelInvoke ("StartConnecting");
		connected = true;
		NetworkStatus.Show ("Connected to server", NetworkStatus.MessageType.success);
	}

	void OnDisconnectedFromServer (NetworkDisconnection info)
	{
		connected = false;
		NetworkStatus.Show ("Server is full or game is running", NetworkStatus.MessageType.failure);
	}

	void ConnectToFound ()
	{
		if (hostData.Length > 1)
			Debug.Log ("More than one servers found");
		
		NetworkStatus.Show ("Connecting to server", NetworkStatus.MessageType.working);
		NetworkConnectionError error = Network.Connect (hostData [0]);
		if (error != NetworkConnectionError.NoError)
			NetworkStatus.Show ("Connection error: " + error, NetworkStatus.MessageType.failure);

	}
}
