using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player
{
	public string name;

	public int body;
	public int barrel;

	public int primary;
	public int secondary;

	public int damage;
	public int rate;
	public int armor;
	public int speed;


	public int kills;
	public int deaths;
	public int assists;
	public int barrels;

	public Player (string name)
	{
		this.name = name;
	}
}
