using UnityEngine;
using System.Collections;

public class LoginHandling : MonoBehaviour 
{
	[RPC]
	public void printA ()
	{
		print ("client A");
	}
}
