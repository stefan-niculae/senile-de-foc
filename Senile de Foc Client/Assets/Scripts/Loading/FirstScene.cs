using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FirstScene : MonoBehaviour 
{
	void Start () 
	{
		//LoadingManager.StartLoading ("Splash");

		PlayerCompareTest ();
	}

	void PlayerCompareTest ()
	{
		Stats noKills = new Stats (0, 10); 		// 0
		Stats noDeaths = new Stats (10, 0);		// Inf
		Stats doneNothing = new Stats (0, 0);	// NaN
		Stats twiceTheKills = new Stats (10, 5);// y / z

		Stats noKillsLessDeaths = new Stats (0, 5);
		Stats noDeathsMoreKills = new Stats (15, 0);
		Stats twiceTheKillsMore = new Stats (100, 50);


		print (5.CompareTo (10) + "-1");

		print ("Zero kills, some deaths:");
		print (noKills.CompareTo (noKills) + "0"); 
		print (noKills.CompareTo (noKillsLessDeaths) + "-1");
		print (noKills.CompareTo (noDeaths) + "-1"); 
		print (noKills.CompareTo (doneNothing) + "-1"); 
		print (noKills.CompareTo (twiceTheKills) + "-1");


		print ("Some kills, zero deaths:"); 
		print (noDeaths.CompareTo (noKills) + "1");
		print (noDeaths.CompareTo (noDeaths) + "0");
		print (noDeaths.CompareTo (noDeathsMoreKills) + "-1"); 
		print (noDeaths.CompareTo (doneNothing) + "1"); 
		print (noDeaths.CompareTo (twiceTheKills) + "1");


		print ("Done nothing:"); 
		print (doneNothing.CompareTo (noKills) + "1");
		print (doneNothing.CompareTo (noDeaths) + "-1");
		print (doneNothing.CompareTo (doneNothing) + "0");
		print (doneNothing.CompareTo (twiceTheKills) + "-1");


		print ("Twice the kills");
		print (twiceTheKills.CompareTo (noKills) + "1");
		print (twiceTheKills.CompareTo (noDeaths) + "-1");
		print (twiceTheKills.CompareTo (doneNothing) + "1");
		print (twiceTheKills.CompareTo (twiceTheKills) + "0");
		print (twiceTheKills.CompareTo (twiceTheKillsMore) + "-1");


		List<Stats> players = new List <Stats> ();
		players.Add (noKills);
		players.Add (noDeaths);
		players.Add (doneNothing);
		players.Add (twiceTheKills);
		players.Add (noKillsLessDeaths);
		players.Add (noDeathsMoreKills);
		players.Add (twiceTheKillsMore);
		players.Sort ((a, b) => -a.CompareTo (b));
		foreach (var p in players)
			print (p);
	}
}
