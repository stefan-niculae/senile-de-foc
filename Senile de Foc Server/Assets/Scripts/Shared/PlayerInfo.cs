using UnityEngine;
using System.Collections;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.IO;
using System.Text;

[System.Serializable]
public class PlayerInfo
{
	public string name;
	public NetworkPlayer networkPlayer;

	public TankType tankType;
	public Rates rates;
	public Stats stats;

	public PlayerInfo (NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;

		tankType = new TankType (-1);
		rates = new Rates (-1);
		stats = new Stats ();
	}

	public override string ToString ()
	{
		return string.Format ("{0}\t{1}\t{2}\t{3}\t{4}", name, networkPlayer.ipAddress, tankType, rates, stats);
	}

	public byte[] ToByteArray ()
	{
		using (MemoryStream ms = new MemoryStream ())
        {
			BinaryFormatter bf = new BinaryFormatter ();
			bf.Serialize (ms, this);
			return ms.ToArray ();
		}
	}

	public static PlayerInfo FromByteArray (byte[] byteArray)
	{
		using (var ms = new MemoryStream ())
		{
			BinaryFormatter bf = new BinaryFormatter ();
			ms.Write (byteArray, 0, byteArray.Length);
			ms.Seek (0, SeekOrigin.Begin);
			PlayerInfo obj = bf.Deserialize (ms) as PlayerInfo;
			return obj;
		}
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

	public TankType (int slotNr, int bodyIndex = -1, int barrelIndex = -1, int primary = -1, int secondary = -1)
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
public class Stats
{
	public int
		kills,
		deaths,
		assists,
		barrels;

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
}