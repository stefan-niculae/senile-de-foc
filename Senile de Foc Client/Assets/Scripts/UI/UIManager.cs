using UnityEngine;
using System.Collections;
using System;

public class UIManager : Singleton<UIManager> 
{
	CameraMovement camMovement;
	GameObject loadingGraphic;

	Transform hidden;
	Transform shown;

	Transform KDPanel;
	Transform playerPanel;
	Transform minimap;
	Transform darkOverlay;
	Transform scoreboard;
	[HideInInspector] public Transform respawn;
	Transform matchOver;
	Transform controlsFrame;

	Transform musicBan;
	Transform soundBan;

	Transform poppedFrame;

	Countdown matchTimer;

	public enum State { loading, playing, dead, alive, matchOver };
	State _state;
	public State state
	{
		get { return _state; }
		set 
		{
			_state = value;

			switch (_state) 
			{
			case State.loading:

				camMovement.enabled = false;
				Vector3 pos = loadingGraphic.transform.position;
				pos.z = Camera.main.transform.position.z;
				Camera.main.transform.position = pos;

				SetVisibility (false, matchOver);

				SetVisibility (false, KDPanel, playerPanel, minimap);
				SetVisibility (false, darkOverlay, scoreboard, respawn, controlsFrame);

				SetVisibility (false, musicBan, soundBan);
				break;


			case State.playing:
				camMovement.enabled = true;

				SetVisibility (true, KDPanel, playerPanel, minimap);
				SetVisibility (true, scoreboard);

				matchTimer.toAppend = " / " + Utils.FloatToTime (Constants.MATCH_DURATION);
				matchTimer.StartIt (Constants.MATCH_DURATION, () => state = State.matchOver);
				break;


			case State.dead:
				SetVisibility (false, KDPanel, playerPanel);
				SetVisibility (true, darkOverlay, respawn);
				break;


			case State.alive:
				SetVisibility (false, darkOverlay, respawn);
				SetVisibility (true, KDPanel, playerPanel);
				break;


			case State.matchOver:
				SetVisibility (false, KDPanel, playerPanel, minimap);
				SetVisibility (false, darkOverlay, respawn, scoreboard);
				SetVisibility (true, matchOver);
				break;
			}
		}
	}

	void Awake ()
	{
		camMovement = Camera.main.GetComponent <CameraMovement> ();
		loadingGraphic = GameObject.Find ("Loading Graphic");

		hidden = Utils.childWithName (transform, "Hidden");
		shown  = Utils.childWithName (transform, "Canvas");

		KDPanel 		= Utils.childWithName (transform, "KD Panel");
		playerPanel 	= Utils.childWithName (transform, "Controlled Player Panel");
		minimap 		= Utils.childWithName (transform, "Minimap");
		darkOverlay 	= Utils.childWithName (transform, "Dark Overlay");
		scoreboard 		= Utils.childWithName (transform, "Scoreboard");
		respawn 		= Utils.childWithName (transform, "Respawn Frame");
		matchOver 		= Utils.childWithName (transform, "Match Over");
		controlsFrame	= Utils.childWithName (transform, "Controls");

		matchTimer 		= Utils.childWithName (scoreboard, "Match Countdown").GetComponent <Countdown> ();

		musicBan 		= Utils.childWithName (transform, "Music Ban");
		soundBan 		= Utils.childWithName (transform, "Sound Ban");

		state = State.loading;
	}

	// Public because ingamesettings also uses this
	public void SetVisibility (bool visible, params Transform[] elements)
	{
		Array.ForEach (elements,
			elem => {
				if (visible)
					elem.transform.position += Constants.HIDDEN;
				else
					elem.transform.position -= Constants.HIDDEN;
//				elem.SetParent (visible ? shown : hidden, false);
//				if (elem == darkOverlay) // overlay should not cover ui elements
//					elem.SetSiblingIndex (1);
				
			});
	}

	public void SetControlsVisibility (bool value)
	{
		poppedFrame = value ? controlsFrame : null;
		SetVisibility (value, controlsFrame);
	}

	public void ShowCredits ()
	{
		print ("Credits: Stefan Niculae - implementation, Adrian Brojbeanu - testing, Hung Trinh - moral support");
	}

	public void SetMusicBan (bool value)
	{
		SetVisibility (value, musicBan);
	}
	public void SetSoundBan (bool value)
	{
		SetVisibility (value, soundBan);
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (poppedFrame == controlsFrame)
				SetControlsVisibility (false);
			else
				IngameSettings.Instance.Toggle ();
		}
	}
}
