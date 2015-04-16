using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour 
{
	// TODO: get this from the login
	public string username;

	TankAttributes.Attributes attributes;

	public enum TankType { heavy, angry, calm, sneaky };
	public TankType tankType;

	// TODO: make all interactions with these through this stats object
	[HideInInspector] public TankHealth health;
	[HideInInspector] public TankMovement movement;
	[HideInInspector] public TankWeapon weapon;
	[HideInInspector] public TankBarrel barrel;
	

	GameObject fireParticlesPrefab;
	Transform fireParticlesParent;

	public float respawnTime = 10f; // in seconds

	public bool controlledPlayer;
	static int nrControlledPlayers;

	[HideInInspector] public GUIStat
		kills,
		deaths,
		assists;

	void Awake ()
	{
		if (controlledPlayer) {
			nrControlledPlayers++;
			if (nrControlledPlayers > 1)
				Debug.LogError ("There are more than one controlled players!");
		}

		PlayerInput playerInput = GetComponent <PlayerInput> ();
		if ((playerInput != null) != controlledPlayer)
			Debug.LogErrorFormat ("Player input missing/where it shouldn't be on {0} ({1})", name, username);

		// player stats -> tank weapon -> tank barrel -> tank bullet -> bullet explosion -> tank health
		kills 	= new GUIStat (controlledPlayer ? GameObject.Find ("Kills Text").GetComponent <Text> () : null);
		deaths 	= new GUIStat (controlledPlayer ? GameObject.Find ("Deaths Text").GetComponent <Text> () : null);
		assists = new GUIStat (controlledPlayer ? GameObject.Find ("Assists Text").GetComponent <Text> () : null);


		// Parsing the selection
		switch (tankType) {
		
		case TankType.heavy:
			attributes = TankAttributes.HEAVY;
			break;

		case TankType.angry:
			attributes = TankAttributes.ANGRY;
			break;

		case TankType.calm:
			attributes = TankAttributes.CALM;
			break;

		case TankType.sneaky:
			attributes = TankAttributes.SNEAKY;
			break;
		}

		fireParticlesPrefab = attributes.fireParticles;
		fireParticlesParent =  Utils.replaceGO (Utils.childWithName (transform, "Fire Particles").gameObject,
		                                        fireParticlesPrefab)
									.transform;


		// Setting up references
		health = GetComponentInChildren <TankHealth> ();
		movement = GetComponentInChildren <TankMovement> ();
		weapon = GetComponentInChildren <TankWeapon> ();
		barrel = GetComponentInChildren <TankBarrel> ();


		// Applying the stats throughout the components
		health.damageAbsorbtion = attributes.damageAbsorbtion;
		health.explosionPrefab = attributes.deathExplosionPrefab;

		movement.forwardSpeed = attributes.forwardSpeed;
		movement.backwardSpeed = attributes.backwardSpeed;
		movement.rotationSpeed = attributes.rotationSpeed;

		barrel.backwardSpeed = attributes.barrelSpeed;
		barrel.forwardSpeed = barrel.backwardSpeed / 2f;

		weapon.cooldownPeriod = attributes.fireCooldown;
		weapon.projectilePrefab = attributes.projectilePrefab;
		weapon.fireParticles = fireParticlesParent.GetComponent <ParticleSystem> ();

		weapon.projectileSpeed = attributes.projectileSpeed;
		weapon.projectileBounces = attributes.projectileBounces;
		weapon.projectileSprite = attributes.projectileSprite;

		weapon.explosionDamage = attributes.explosionDamage;
		weapon.explosionRadius = attributes.explosionRadius;

		Utils.childWithName (transform, "Body").GetComponent <SpriteRenderer> ().sprite = attributes.bodySprite;
		Utils.childWithName (transform, "Barrel").GetComponent <SpriteRenderer> ().sprite = attributes.barrelSprite;
	}
}

public class GUIStat
{
	Text textHandle;

	int _amount;
	public int amount
	{
		get 
		{
			return _amount;
		}

		set
		{
			_amount = value;

			// The texthandle can be null because only the user that plays on this machine sees the updates on screen
			if (textHandle != null)
				textHandle.text = amount.ToString ();
		}
	}

	// Oh operator overloading, how I've missed thee
	public static GUIStat operator + (GUIStat stat, int nr)
	{
		stat.amount += nr;
		return stat;
	}

	public static GUIStat operator - (GUIStat stat, int nr)
	{
		stat.amount -= nr;
		return stat;
	}

	public static GUIStat operator ++ (GUIStat stat)
	{
		stat.amount++;
		return stat;
	}

	public static GUIStat operator -- (GUIStat stat)
	{
		stat.amount--;
		return stat;
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
