using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AboveInfo : Containable<AboveInfo> 
{
	Text username;
	[HideInInspector] public HealthBar health;
	ParticleSystem locatorParticles;
	TankInfo player;

	const float TOP_MARGIN = 1.85f;

	void Awake ()
	{
		moveToContainer ("Above Infos");

		username 	= Utils.childWithName (transform, "Name")		.GetComponent <Text> ();
		health		= Utils.childWithName (transform, "Health Bar")	.GetComponent <HealthBar> ();
	}

	public void Initialize (TankInfo player)
	{
		this.player = player;
		locatorParticles = Utils.childWithName (player.transform, "Own Tank Locator").GetComponent <ParticleSystem> ();
		username.text = player.playerInfo.name;

		// The controlled player has the big static health bar usernamets own
		if (player.isMine)
			health.gameObject.SetActive (false);
	}

	void Update ()
	{
		HandleVisibility ();
		FollowPlayer ();
	}

	void HandleVisibility ()
	{
		if (Input.GetKey (KeyCode.LeftShift)) {
			username.enabled = true;
			if (player.isMine)
				locatorParticles.enableEmission = true;
		}
		else {
			username.enabled = false;
			locatorParticles.enableEmission = false;
		}
	}

	void FollowPlayer ()
	{
		if (player != null) {
			var pos = (Vector2)Camera.main.WorldToScreenPoint (player.transform.position);

			var maxY = Camera.main.WorldToScreenPoint (new Vector2 (0, GameWorld.maxTop - TOP_MARGIN)).y;
			if (pos.y > maxY)
				pos.y = maxY;
			
			transform.position = pos;
		}
	}
}
