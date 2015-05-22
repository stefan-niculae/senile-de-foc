using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour 
{
	[HideInInspector] public string username;

	Text usernameText;
	Image body;
	Image barrel;
	Image checkmark;
	Image highlight;

	void Awake ()
	{
		usernameText = Utils.childWithName (transform, "Username").GetComponent <Text> ();
		body = Utils.childWithName (transform, "Body").GetComponent <Image> ();
		barrel = Utils.childWithName (transform, "Barrel").GetComponent <Image> ();
		checkmark = Utils.childWithName (transform, "Ready Checkmark").GetComponent <Image> ();
		highlight = Utils.childWithName (transform, "Highlight").GetComponent <Image> ();
	}

	public void Init (string username, Sprite tankBody, Sprite tankBarrel)
	{
		this.username = username;
		usernameText.text = username;

		body.sprite = tankBody;
		barrel.sprite = tankBarrel;

		if (username != SplashMenus.currentUsername)
			highlight.enabled = false;
	}

	public void MakeReady (Sprite readyCheckmark)
	{
		checkmark.sprite = readyCheckmark;
	}
}
