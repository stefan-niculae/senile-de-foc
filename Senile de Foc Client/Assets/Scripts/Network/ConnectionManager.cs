using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour
{
	static readonly string SERVER_IP = "25.34.150.10";
	static readonly int PORT = 25001;

	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			GUI.Label(new Rect(10, 10, 200, 20), "Status: Disconnected");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Client Connect"))
			{
				Network.Connect(SERVER_IP, PORT);

			}
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Client");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect(200);
			}
		}

	}
}
		
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Net;
//using System.Net.Sockets;
//
//public class ConnectionManager : MonoBehaviour 
//{
//	static readonly string SERVER_IP = "10.240.2.184";
//	static readonly int PORT = 5588;
//
//	static readonly int BUFFER_SIZE = 1024;
//
//	IPEndPoint ipep = null;
//	Socket serverSocket = null;
//	Thread listenerThread = null;
//	Thread connectThread = null;
//
//	bool listening = false;
//	byte[] receiveBuffer;
//
//	void Start ()
//	{
//		ConnectToServer ();
//	}
//
//	/**
//	 * Returns whether the connection was successful
//	 */
//	void ConnectToServer ()
//	{
//		ipep = new IPEndPoint (IPAddress.Parse (SERVER_IP), PORT);
//		serverSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//
//		connectThread = new Thread (() => Connect ());
//		connectThread.Start ();
//	}
//
//	void Connect ()
//	{
//		try {
//			Debug.Log ("Trying to connect to " + ipep);
//			serverSocket.Connect (ipep);
//		} catch (SocketException e) {
//			Debug.LogError ("Connection failed\n" + e.StackTrace);
//			HandleFailedConnection ();
//		}
//		HandleSuccesfullConnection ();
//	}
//
//	void HandleSuccesfullConnection ()
//	{
//		Debug.Log ("Connection successful!!");
//		StartListening ();
//	}
//
//	void HandleFailedConnection ()
//	{
//		Debug.Log ("Connection failed!!");
//	}
//
//	void StartListening ()
//	{
//		listening = true;
//		receiveBuffer = new byte[BUFFER_SIZE];
//		listenerThread = new Thread (() => Listen ());
//		listenerThread.Start ();
//	}
//
//	void Listen ()
//	{
//		while (listening) {
//			int bytesReceived = serverSocket.Receive (receiveBuffer);
//			string response = Encoding.ASCII.GetString (receiveBuffer, 0, bytesReceived);
//			HandleResponse (response);
//		}
//	}
//
//	void HandleResponse (string response)
//	{
//		Debug.Log (response);
//	}
//
//	/**
//	 * Returns whether the entire message was sent
//	 */
//	public void Send (string message)
//	{
//		byte[] sendBuffer = Encoding.ASCII.GetBytes (message);
//		int bytesSent = serverSocket.Send (sendBuffer);
////		return bytesSent == sendBuffer.Length;
//	}
//}

