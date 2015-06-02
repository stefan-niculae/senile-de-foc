using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HitterInfo : BasicInfo 
{
	Text damagePer;

	public override void AdditionalReferencesSet ()
	{
		damagePer = Utils.childWithName (transform, "Damage Percentage").GetComponent <Text> ();
	}

	public override void AdditionalValuesSet (PlayerInfo playerInfo)
	{ }

	public void SetDamagePer (float amount)
	{
		int rounded = Mathf.RoundToInt (amount);
		damagePer.text = rounded + "%";
	}
}
