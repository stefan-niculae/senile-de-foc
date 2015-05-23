using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour 
{
	[HideInInspector] public string username;

	Text usernameText;
	Image checkmark;
	Image highlight;

	
	MirrorImage bodyMirror;
	MirrorImage barrelMirror;

	void Awake ()
	{
		usernameText = Utils.childWithName (transform, "Username").GetComponent <Text> ();
		bodyMirror = Utils.childWithName (transform, "Body").GetComponent <MirrorImage> ();
		barrelMirror = Utils.childWithName (transform, "Barrel").GetComponent <MirrorImage> ();
		checkmark = Utils.childWithName (transform, "Ready Checkmark").GetComponent <Image> ();
		highlight = Utils.childWithName (transform, "Highlight").GetComponent <Image> ();
	}

	public void Init (string username, Image tankBody, Image tankBarrel)
	{
		this.username = username;
		usernameText.text = username;

		bodyMirror.target = tankBody;
		barrelMirror.target = tankBarrel;

		if (username != SplashMenus.currentUsername)
			highlight.enabled = false;
	}

	public void MakeReady (Sprite readyCheckmark)
	{
		checkmark.sprite = readyCheckmark;
	}
}
