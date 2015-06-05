using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankInfo : MonoBehaviour 
{
	public PlayerInfo playerInfo;
	public bool isMine;
	public IngameInfo scoreboardInfo;

	[HideInInspector] public TankHealth health;
	[HideInInspector] public TankMovement movement;
	[HideInInspector] public Cannon cannon;
	[HideInInspector] public TankBarrel barrel;
	[HideInInspector] public Projectile projectile;
	[HideInInspector] public Secondary special;
	[HideInInspector] public ExplosionStats cannonProjectileExplosionStats;
	[HideInInspector] public PlayerInput input;
	[HideInInspector] public TankSounds sounds;
	

	AboveInfo aboveInfo;
	public GUIStat
		kills,
		deaths;


	void Awake ()
	{
		// Setting up references
		health 				= GetComponent <TankHealth> ();
		movement 			= GetComponent <TankMovement> ();
		cannon 				= GetComponent <Cannon> ();
		barrel 				= GetComponentInChildren <TankBarrel> ();
		projectile 			= cannon.effectPrefab.GetComponent <Projectile> ();
		special 			= GetComponent <Secondary> ();
		sounds 				= GetComponentInChildren <TankSounds> ();
	}

	public void Initialize (PlayerInfo playerInfo, bool isMine)
	{
		this.playerInfo = playerInfo;
		this.isMine = isMine;

		// player stats -> tank weapon -> tank barrel -> tank bullet -> bullet explosion -> tank health
		kills 	= new GUIStat (isMine ? GameObject.Find ("Kills Text").GetComponent <Text> () : null);
		deaths 	= new GUIStat (isMine ? GameObject.Find ("Deaths Text").GetComponent <Text> () : null);

		if (isMine) {
			health.bar = GameObject.Find ("Static Health Bar").GetComponent <HealthBar> ();
			Camera.main.GetComponent <CameraMovement> ().player = transform;
			Minimap.Instance.controlledPlayer = transform;

			cannon .cooldownFill = GameObject.Find ("Projectile Cooldown Fill").GetComponent <Image> ();
			special.cooldownFill = GameObject.Find ("Secondary Cooldown Fill") .GetComponent <Image> ();

			GameObject.Find ("Projectile Glyph").GetComponent <Image> ().sprite = References.Instance.primarySprites   [playerInfo.tankType.projectileType];
			GameObject.Find ("Secondary Glyph") .GetComponent <Image> ().sprite = References.Instance.secondarySprites [playerInfo.tankType.secondary];

			health.camMovement = Camera.main.GetComponent <CameraMovement> ();
			health.respawnCountdown = GameObject.Find ("Respawn Time Text").GetComponent <Countdown> ();


			input = GetComponent <PlayerInput> ();
			input.enabled = true;
		}


		health.RegisterThis (playerInfo.orderNumber);

		var aboveObj = GameObject.Instantiate (References.Instance.aboveInfoPrefab) as GameObject;
		aboveInfo = aboveObj.GetComponent <AboveInfo> ();
		aboveInfo.Initialize (this);

		if (!isMine)
			health.bar = aboveInfo.health;
		

		ApplyRates ();
		ApplySprites ();
	}



	void ApplyRates () 
	{
		// Each tank has its own projectile (missle or bullet)
		cannon .effectPrefab = References.Instance.projectilePrefabs [playerInfo.tankType.projectileType];
		// Each tank has its own special effect, except for the custom one
		special.effectPrefab = References.Instance.specialPrefabs    [playerInfo.tankType.secondary];

		// We multiply by two because in the level select there are only 5 beans
		// but computing rates espects values 0 - 10

		health.damageAbsorbtion = 	TankAttributes.Instance.damageAbsorbtion	.compute (playerInfo.rates.armor * 2);
		
		movement.forwardSpeed = 	TankAttributes.Instance.forwardSpeed		.compute (playerInfo.rates.speed * 2);
		movement.backwardSpeed = 	TankAttributes.Instance.backwardSpeed		.compute (playerInfo.rates.speed * 2);
		movement.rotationSpeed = 	TankAttributes.Instance.rotationSpeed		.compute (playerInfo.rates.speed * 2);
		
		cannon.fireRate = 			TankAttributes.Instance.fireRate			.compute (playerInfo.rates.fireRate * 2);
		barrel.backwardSpeed = 		TankAttributes.Instance.barrelSpeed			.compute (playerInfo.rates.fireRate * 2);
		barrel.forwardSpeed = 		barrel.backwardSpeed / 2f;
		projectile.speed = 			TankAttributes.Instance.projectileSpeed		.compute (playerInfo.rates.fireRate * 2);
		
		cannonProjectileExplosionStats.damage =TankAttributes.Instance.explosionDamage		.compute (playerInfo.rates.damage * 2);	
		cannonProjectileExplosionStats.radius =TankAttributes.Instance.explosionRadius		.compute (playerInfo.rates.damage * 2);
	}

	void ApplySprites ()
	{
		movement.gameObject.GetComponent <SpriteRenderer> ().sprite 	= References.Instance.bodySprites [playerInfo.tankType.bodyIndex];
		barrel.gameObject.GetComponent <SpriteRenderer> ().sprite		= References.Instance.barrelSprites [playerInfo.tankType.barrelIndex];
	}

//	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
//	{
//		stream.Serialize (ref playerInfo.tankType.bodyIndex);
//		stream.Serialize (ref playerInfo.tankType.barrelIndex);
//
//		if (stream.isReading)
//			ApplySprites ();
//	}

	public void ShowStatsRecap ()
	{
		kills.amount  = playerInfo.stats.kills;
		deaths.amount = playerInfo.stats.deaths;
	}

}

[System.Serializable]
public class GUIStat
{
	Text textHandle;

	int _amount;
	public int amount
	{
		get 
		{ return _amount; }

		set
		{
			_amount = value;
		
			// The texthandle can be null because only the user that plays on this machine sees the updates on screen
			if (textHandle != null)
				textHandle.text = amount.ToString ();
		}
	}
	
	public GUIStat (Text textHandle)
	{
		this.textHandle = textHandle;
		Reset ();
	}

	public void Reset ()
	{
		amount = 0;
	}
}
