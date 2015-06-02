using UnityEngine;
using System.Collections;
using System;

public class Constants : MonoBehaviour 
{
	public const int MAX_PLAYERS = 4;

	// Don't forget to set the z coordinate before using this
	public static readonly Vector3 HIDDEN = new Vector2 (10000f, 10000f);

	// For comparisons of imprecise data (time, distance)
	public const float EPSILON = .01f;
	
	// For REALLY imprecise calculations
	public const float BIG_EPSILON = 1.5f;

	// Failsafe for while (random == smthing)
	public const int MAX_EPOCH = 1000;

	public const float DOT_RATE = .3f; // in seconds


	public static readonly Action DO_NOTHING = new Action (() => { });

	public const float SMALL_DURATION = .25f;


	public const int PTS_TO_SPEND = 12;


	public const float MATCH_DURATION = 60 * 5; // in seconds

	public const float TANK_RESPAWN_TIME = 10; // in seconds
	public const float BARREL_RESPAWN_TIME = 5; // in seconds

	public const float SECONDARY_FIRE_RATE = 2; // in seconds
}
