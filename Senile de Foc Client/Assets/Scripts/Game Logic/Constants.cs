using UnityEngine;
using System.Collections;
using System;

public class Constants : MonoBehaviour 
{
	public static readonly int MAX_PLAYERS = 4;

	// Don't forget to set the z coordinate before using this
	public static readonly Vector3 HIDDEN = new Vector2 (1000f, 1000f);

	// For comparisons of imprecise data (time, distance)
	public static readonly float EPSILON = .01f;
	
	// For REALLY imprecise calculations
	public static readonly float BIG_EPSILON = 1.5f;

	// Failsafe for while (random == smthing)
	public static readonly int MAX_EPOCH = 1000;

	public static readonly float DOT_RATE = .3f; // in seconds


	public static readonly Action DO_NOTHING = new Action (() => { });

	public static readonly float SMALL_DURATION = .25f;


	public static int PTS_TO_SPEND = 12;
}
