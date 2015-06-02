using UnityEngine;
using System.Collections;
using System;


[System.Serializable]
public class PlayerInfo : IComparable
{
	public string name;
	public string ip;
	public bool ready;
	public bool loadedGame;
	public int orderNumber;
	
	public TankType tankType;
	public Rates rates;
	public Stats stats;
	
	public PlayerInfo (NetworkPlayer networkPlayer)
	{
		ip = networkPlayer.ipAddress;
		
		Reset ();
	}
	
	public override string ToString ()
	{
		return string.Format ("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", name, ready ? "R" : "N", ip, tankType, rates, stats);
	}

	public int CompareTo (object obj)
	{
		if (obj == null)
			return 1;

		PlayerInfo other = obj as PlayerInfo;
		if (other != null)
			return this.stats.CompareTo (other.stats);
		else
			throw new ArgumentException ("Object is not a Player Info");
	}
	
	public void Reset ()
	{
		name = "";
		ready = false;
		loadedGame = false;
		orderNumber = NetworkConstants.NOT_SET;
		
		rates = new Rates (NetworkConstants.NOT_SET);
		tankType = new TankType (NetworkConstants.NOT_SET);
		stats = new Stats ();
	}
}

[System.Serializable]
public class TankType
{
	public int
		slotNr,
		bodyIndex,
		barrelIndex,
		primary,
		secondary;

	public int projectileType
	{
		get
		{
			int res = slotNr;
			if (slotNr == 4) 		// custom tank
				res += primary;  	// missile (0) or bullet (1)
			return res;
		}
	}
	
	public TankType (int slotNr, int bodyIndex 	= NetworkConstants.NOT_SET, 
	                 int barrelIndex= NetworkConstants.NOT_SET, 
	                 int primary 	= NetworkConstants.NOT_SET, 
	                 int secondary 	= NetworkConstants.NOT_SET)
	{
		this.slotNr = slotNr;
		
		if (slotNr >= 0 && slotNr < 4)
			bodyIndex =
				barrelIndex = slotNr;
		
		if (slotNr == 0 || slotNr == 1)
			primary = 0;
		if (slotNr == 2 || slotNr == 3)
			primary = 1;
		
		if (slotNr >= 0 && slotNr < 4)
			secondary  = slotNr;
		
		this.bodyIndex = bodyIndex;
		this.barrelIndex = barrelIndex;
		this.primary = primary;
		this.secondary = secondary;
	}
	
	public override string ToString ()
	{
		return string.Format ("{0}\t{1}\t{2}\t{3}\t{4}", slotNr, bodyIndex, barrelIndex, primary, secondary);
	}
	
}

[System.Serializable]
public class Rates
{
	public int
		damage,
		fireRate,
		armor,
		speed;
	
	public Rates (int presetType)
	{
		if (presetType == 0) {
			damage 	= 4;
			fireRate= 2;
			armor 	= 3;
			speed 	= 3;
		}
		if (presetType == 1) {
			damage 	= 5;
			fireRate= 1;
			armor 	= 4;
			speed 	= 2;
		}
		if (presetType == 2) {
			damage 	= 2;
			fireRate= 4;
			armor 	= 3;
			speed 	= 3;
		}
		if (presetType == 3) {
			damage 	= 1;
			fireRate= 5;
			armor 	= 2;
			speed 	= 4;
		}
	}
	
	public Rates (int damage, int fireRate, int armor, int speed)
	{
		this.damage = damage;
		this.fireRate = fireRate;
		this.armor = armor;
		this.speed = speed;
	}
	
	public override string ToString ()
	{
		return string.Format ("{0}\t{1}\t{2}\t{3}", damage, fireRate, armor, speed);
	}
}

[System.Serializable]
public class Stats : IComparable
{
	public int
		kills,
		deaths,
		assists,
		barrels;

	public float KDratio
	{ get { return (float)kills / deaths; } }
	
	public Stats ()
	{
		kills =
		deaths = 
		assists = 
		barrels = 0;
	}
	
	public override string ToString ()
	{
		return string.Format ("{0}\t{1}\t{2}\t{3}", kills, deaths, assists, barrels);
	}

	// TODO tests
	/**
	 * 		< 0 	this < obj
	 * 		= 0 	this = obj
	 * 		> 0 	this > obj
	 */
	public int CompareTo (object obj)
	{
		if (obj == null)
			return 1;

		Stats other = obj as Stats;
		if (other != null) {
			float rt = this.KDratio;
			float ro = other.KDratio;

			// let x, y, z be nonzero floats
			// 0 	means 0 / x
			// inf	means x / 0
			// NaN  means 0 / 0
			// x    means y / z

			if (rt == 0) {
				if (ro == 0)
					return this.deaths.CompareTo (other.deaths) * -1;
				else if (float.IsInfinity (ro))
					return +1;
				else if (float.IsNaN (ro))
					return +1;
				else
					return +1;
			}
			else if (float.IsInfinity (rt)) {
				if (ro == 0)
					return -1;
				else if (float.IsInfinity (ro))
					return this.kills.CompareTo (other.kills);
				else if (float.IsNaN (ro))
					return -1;
				else
					return +1;
			}
			else if (float.IsNaN (rt)) {
				if (ro == 0)
					return -1;
				else if (float.IsInfinity (ro))
					return +1;
				else if (float.IsNaN (ro))
					return  0;
				else
					return +1;
			}
			else {
				if (ro == 0)
					return -1;
				else if (float.IsInfinity (ro))
					return +1;
				else if (float.IsNaN (ro))
					return -1;
				else
					return this.kills.CompareTo (other.kills);
			}
		}
		else
			throw new ArgumentException ("Object is not a Stat");
	}
}
