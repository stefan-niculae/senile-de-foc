using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IngameInfo : BasicInfo 
{
	Text kills;
	Text deaths;
	Text assists;
	Text barrels;
	Text rank;

	[HideInInspector] public Countdown respawn;

	public override void AdditionalReferencesSet ()
	{
		kills 	= Utils.childWithName (transform, "Kills")	.GetComponent <Text> ();
		deaths 	= Utils.childWithName (transform, "Deaths")	.GetComponent <Text> ();
		assists = Utils.childWithName (transform, "Assists").GetComponent <Text> ();
		barrels = Utils.childWithName (transform, "Barrels").GetComponent <Text> ();
		rank    = Utils.childWithName (transform, "Rank")   .GetComponent <Text> ();
		respawn = Utils.childWithName (transform, "Respawn").GetComponent <Countdown> ();
	}

	public override void AdditionalValuesSet (PlayerInfo playerInfo)
	{
		kills.text 		= playerInfo.stats.kills.ToString ();
		deaths.text 	= playerInfo.stats.deaths.ToString ();
		assists.text 	= playerInfo.stats.assists.ToString ();
		barrels.text	= playerInfo.stats.barrels.ToString ();
	}

	public void SetRank (int rank)
	{
		this.rank.text = rank;
	}
}
