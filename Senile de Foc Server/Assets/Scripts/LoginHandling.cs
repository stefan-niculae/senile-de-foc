using UnityEngine;
using System.Collections;

public class LoginHandling : MonoBehaviour 
{
	[RPC]
	public void DoLogin (string username, string password)
	{
		print (username + " " + password + " logged in");
	}
}
