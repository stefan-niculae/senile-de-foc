using UnityEngine;
using System.Collections;

public class PlayerUsername : MonoBehaviour
{
	PlayerStats stats;

	TextMesh textHandle;
	bool visible;

	void Awake ()
	{
		textHandle = GetComponent <TextMesh> ();
		stats = GetComponentInParent <PlayerStats> ();
	}

	void Update ()
	{
		// Only show the Usernames when holding U
		visible = Input.GetKey (KeyCode.U);

		textHandle.text = visible ? stats.username : "";
	}
}
