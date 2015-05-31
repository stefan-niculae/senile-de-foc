using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class BasicInfo : MonoBehaviour 
{
	Text username;
	Image highlight;
	Image body;
	Image barrel;

	void Awake ()
	{
		BasicReferencesSet ();
	}

	void BasicReferencesSet () 
	{
		username 	= Utils.childWithName (transform, "Username")		.GetComponent <Text> ();
		body 		= Utils.childWithName (transform, "Body")			.GetComponent <Image> ();
		barrel		= Utils.childWithName (transform, "Barrel")			.GetComponent <Image> ();
		highlight	= Utils.childWithName (transform, "Highlight")		.GetComponent <Image> ();

		AdditionalReferencesSet ();
	}

	public abstract void AdditionalReferencesSet ();
	
	public void SetValues (PlayerInfo playerInfo)
	{
		if (string.IsNullOrEmpty (playerInfo.name))
			playerInfo.name = "-";
		username.text 	= playerInfo.name;

		
		if (playerInfo.tankType.bodyIndex == NetworkConstants.NOT_SET)
			body.sprite 	= References.Instance.invisibleSprite;
		else
			body.sprite 	= References.Instance.bodySprites [playerInfo.tankType.bodyIndex];
		
		if (playerInfo.tankType.barrelIndex == NetworkConstants.NOT_SET)
			barrel.sprite = References.Instance.invisibleSprite;
		else 
			barrel.sprite = References.Instance.barrelSprites [playerInfo.tankType.barrelIndex];
		
		
		if (username.text != SplashMenus.currentUsername)
			highlight.enabled = false;
		
		AdditionalValuesSet (playerInfo);
	}

	public abstract void AdditionalValuesSet (PlayerInfo playerInfo);
}
