using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour 
{
	// TODO: get this from the login
	public string username;

	public bool controlledPlayer;
	public Text
		killsText,
		deathsText,
		assistsText;

	[HideInInspector] public GUIStat
		kills,
		deaths,
		assists;

	void Awake ()
	{
		// player stats -> tank weapon -> tank barrel -> tank bullet -> bullet explosion -> tank health
		kills 	= new GUIStat (killsText);
		deaths 	= new GUIStat (deathsText);
		assists = new GUIStat (assistsText);
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
