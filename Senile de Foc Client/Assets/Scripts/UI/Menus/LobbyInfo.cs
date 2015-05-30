using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyInfo : BasicInfo
{	
	Image checkmark;

	public override void AdditionalReferencesSet ()
	{
		checkmark 	= Utils.childWithName (transform, "Ready Checkmark").GetComponent <Image> ();
	}

	public override void AdditionalValuesSet (PlayerInfo playerInfo)
	{
		if (playerInfo.ready)
			checkmark.sprite = References.Instance.readyCheckmark;
	}

}
