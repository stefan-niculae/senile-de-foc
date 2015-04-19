using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour 
{
	public bool move = true; // for debuggin purposes
	public bool controlledPlayer;
	static int nrControlledPlayers;
	// TODO: get this from the login
	public string username;
	
	public enum TankType { abstractTank, heavy, angry, calm, sneaky };
	public TankType tankType;
	public Levels levels;

	[HideInInspector] public TankHealth health;
	[HideInInspector] public TankMovement movement;
	[HideInInspector] public TankWeapon weapon;
	[HideInInspector] public TankBarrel barrel;
	[HideInInspector] public Projectile projectile;
	[HideInInspector] public Explosion projectileExplosion;

	public GUIStat
		kills,
		deaths,
		assists,
		barrels;

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
		barrels = new GUIStat (null); // TODO: show in the scoreboard menu


		// Setting up references
		health = GetComponentInChildren <TankHealth> ();
		movement = GetComponentInChildren <TankMovement> ();
		weapon = GetComponentInChildren <TankWeapon> ();
		barrel = GetComponentInChildren <TankBarrel> ();
		projectile = weapon.projectilePrefab.GetComponent <Projectile> ();
		projectileExplosion = projectile.explosionPrefab.GetComponent <Explosion> ();

		if (controlledPlayer)
			health.bar = GameObject.Find ("Static Health Bar").GetComponent <HealthBar> ();
		else
			health.bar = Dispenser.TakeHealthBar (this);

		levels.Apply (health, movement, weapon, barrel, projectile, projectileExplosion);
	}
}

[System.Serializable]
public class GUIStat
{
	Text textHandle;

	public int _amount; // just for the inspector
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
				textHandle.text = amount.ToString (); // TODO: make this bounce a little!
		}
	}

	// Oh operator overloading, how I've missed thee
	public static GUIStat operator ++ (GUIStat stat)
	{
		stat.amount++;
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
