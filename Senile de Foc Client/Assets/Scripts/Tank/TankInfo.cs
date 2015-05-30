using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankInfo : MonoBehaviour 
{
	public PlayerInfo playerInfo;
	public bool isMine;

	[HideInInspector] public TankHealth health;
	[HideInInspector] public TankMovement movement;
	[HideInInspector] public Cannon cannon;
	[HideInInspector] public TankBarrel barrel;
	[HideInInspector] public Projectile projectile;
	[HideInInspector] public Explosion projectileExplosion;
	[HideInInspector] public Secondary special;
	

	AboveInfo aboveInfo;
	public GUIStat
		kills,
		deaths;


	void Awake ()
	{
		// Setting up references
		health 				= GetComponentInChildren <TankHealth> ();
		movement 			= GetComponentInChildren <TankMovement> ();
		cannon 				= GetComponentInChildren <Cannon> ();
		barrel 				= GetComponentInChildren <TankBarrel> ();
		projectile 			= cannon.effectPrefab.GetComponent <Projectile> (); // TODO set this according to type
		projectileExplosion = projectile.explosionPrefab.GetComponent <Explosion> ();
		special 			= GetComponentInChildren <Secondary> ();
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

		}

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
		
		projectileExplosion.damage =TankAttributes.Instance.explosionDamage		.compute (playerInfo.rates.damage * 2);
		projectileExplosion.radius =TankAttributes.Instance.explosionRadius		.compute (playerInfo.rates.damage * 2);
	}

	void ApplySprites ()
	{
		movement.gameObject.GetComponent <SpriteRenderer> ().sprite 	= References.Instance.bodySprites [playerInfo.tankType.bodyIndex];
		barrel.gameObject.GetComponent <SpriteRenderer> ().sprite		= References.Instance.barrelSprites [playerInfo.tankType.barrelIndex];
	}

	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		stream.Serialize (ref playerInfo.tankType.bodyIndex);
		stream.Serialize (ref playerInfo.tankType.barrelIndex);

		print ("on serialize network view: sender = " + info.sender.ipAddress + ", is writing = " + stream.isWriting);

		if (stream.isReading)
			ApplySprites ();
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

	// Oh operator overloading, how I've missed thee
	public static GUIStat operator ++ (GUIStat stat)
	{
		stat.amount++;
		return stat;
	}

	public void Reset ()
	{
		amount = 0;
	}
}
