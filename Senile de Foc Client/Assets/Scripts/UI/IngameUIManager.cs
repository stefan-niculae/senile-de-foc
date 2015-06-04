using UnityEngine;
using System.Collections;

public class IngameUIManager : UIManager
{
	const float MIN_CAM_ZOOM = 3;
	const float MAX_CAM_ZOOM = 8.5;
	const float CAM_ZOOM_STEP = -.5f;

	CameraMovement camMovement;
	GameObject loadingGraphic;

	Transform settings;
	Transform KDPanel;
	Transform playerPanel;
	Transform minimap;
	Transform darkOverlay;
	Transform scoreboard;
	[HideInInspector] public Transform respawn;
	Transform matchOver;
	Transform controls;

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
				SetVisibility (false, darkOverlay, scoreboard, respawn, controls);
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
				SetVisibility (false, KDPanel, playerPanel, minimap, settings);
				SetVisibility (false, respawn, scoreboard);
				SetVisibility (true, matchOver, darkOverlay);

				MatchOver.Instance.YankPlayers ();
				Scoreboard.Instance.enabled = false;
				GameServer.Instance.orderNrToTankInfo [GameServer.selfInfo.orderNumber].input.enabled = false;
				break;
			}
		}
	}

	public override void AwakeRferences ()
	{
		camMovement = Camera.main.GetComponent <CameraMovement> ();
		loadingGraphic = GameObject.Find ("Loading Graphic");

		settings		= Utils.childWithName (transform, "Settings");
		KDPanel 		= Utils.childWithName (transform, "KD Panel");
		playerPanel 	= Utils.childWithName (transform, "Controlled Player Panel");
		minimap 		= Utils.childWithName (transform, "Minimap");
		darkOverlay 	= Utils.childWithName (transform, "Dark Overlay");
		scoreboard 		= Utils.childWithName (transform, "Scoreboard");
		respawn 		= Utils.childWithName (transform, "Respawn Frame");
		matchOver 		= Utils.childWithName (transform, "Match Over");
		controls		= Utils.childWithName (transform, "Controls");

		matchTimer 		= Utils.childWithName (scoreboard, "Match Countdown").GetComponent <Countdown> ();
		Scoreboard.Instance.respawn = respawn;

		state = State.loading;
	}

	public override void OnVisibilityChange (Transform elem, bool visible)
	{
		// If you want to take a look at the controls and you are respawning, the respawn goes away
		if (elem == controls &&  visible && Mathf.Abs (respawn.position.y) < Constants.HIDDEN.y / 2)
			respawn.position = new Vector3 (respawn.position.x, respawn.position.y + Constants.HIDDEN.y, 0);
		if (elem == controls && !visible && respawn.position.y >  Constants.HIDDEN.y / 2)
			respawn.position = new Vector3 (respawn.position.x, respawn.position.y - Constants.HIDDEN.y, 0);
	}
		
	public void SetControlsVisibility (bool value)
	{
		poppedFrame = value ? controls : null;
		SetVisibility (value, controls);
	}

	public void ShowCredits ()
	{
		print ("Credits: Stefan Niculae - implementation, Adrian Brojbeanu - documentation, Hung Trinh - testing");
	}

	public bool ClearPopup ()
	{
		if (poppedFrame != null) {
			SetVisibility (false, poppedFrame);
			poppedFrame = null;
			return true;
		}
		return false;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!ClearPopup ())
				Settings.Instance.Toggle ();
		}

		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0) {
			float newZoom = Camera.main.orthographicSize + scroll * CAM_ZOOM_STEP;
			Camera.main.orthographicSize = Mathf.Clamp (newZoom, MIN_CAM_ZOOM, MAX_CAM_ZOOM);
		}
	}
}
