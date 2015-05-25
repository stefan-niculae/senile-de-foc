using UnityEngine;
using System.Collections;

public class NameAbove : MonoBehaviour
{
	PlayerStats stats;

	TextMesh textHandle;
	GameObject ownTankLocator;

	bool visible;

	void Awake ()
	{
		textHandle = GetComponent <TextMesh> ();
		stats = GetComponentInParent <PlayerStats> ();
		ownTankLocator = Utils.childWithName (stats.transform, "Own Tank Locator").gameObject;
	}

	void Update ()
	{
		// Only show the Usernames when holding U
		visible = Input.GetKey (KeyCode.U);

		textHandle.text = visible ? stats.username : "";
		ownTankLocator.SetActive (visible && stats.controlledPlayer);
	}
}
