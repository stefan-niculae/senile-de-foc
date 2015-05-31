using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IngameInfo : BasicInfo 
{
	Text kills;
	Text deaths;
	Text assists;
	Text barrels;

	Text respawn;

	public override void AdditionalReferencesSet ()
	{
		kills 	= Utils.childWithName (transform, "Kills")	.GetComponent <Text> ();
		deaths 	= Utils.childWithName (transform, "Deaths")	.GetComponent <Text> ();
		assists = Utils.childWithName (transform, "Assists").GetComponent <Text> ();
		barrels = Utils.childWithName (transform, "Barrels").GetComponent <Text> ();
		respawn = Utils.childWithName (transform, "Respawn").GetComponent <Text> ();
	}

	public override void AdditionalValuesSet (PlayerInfo playerInfo)
	{
		kills.text 		= playerInfo.stats.kills.ToString ();
		deaths.text 	= playerInfo.stats.deaths.ToString ();
		assists.text 	= playerInfo.stats.assists.ToString ();
		barrels.text	= playerInfo.stats.barrels.ToString ();

		//TODO respawn time here
	}

}
