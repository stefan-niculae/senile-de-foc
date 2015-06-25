using UnityEngine;
using System.Collections;

public class IngameUIManager : UIManager
{
	CameraMovement camMovement;
	GameObject loadingGraphic;

	public static bool _pointerOverButton;
	public bool pointerOverButton 
	{
		get { return pointerOverButton; }
		set { _pointerOverButton = value; }
	}

	Transform settings;
	Transform KDPanel;
	Transform playerPanel;
	Transform minimap;
	Transform darkOverlay;
	Transform scoreboard;
	[HideInInspector] public Transform respawn;
	Transform matchOver;
	Transform controls;
	Transform quitConfirmation;
	Transform credits;
	Transform help;

	Transform shownFrame;

	Countdown matchTimer;

	const float MIN_CAM_ZOOM = 3;
	const float MAX_CAM_ZOOM = 8.5f;
	const float CAM_ZOOM_STEP = -.5f;

	const string CAM_ZOOM_KEY = "cam zoom";
	float cameraZoom
	{
		get { return PlayerPrefs.GetFloat (CAM_ZOOM_KEY, 5); }
		set 
		{ 
			value = Mathf.Clamp (value, MIN_CAM_ZOOM, MAX_CAM_ZOOM);
			PlayerPrefs.SetFloat (CAM_ZOOM_KEY, value); 
			Camera.main.orthographicSize = value;
		}
	}


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
				SetVisibility (false, darkOverlay, scoreboard, respawn, controls, quitConfirmation, credits, help);
				break;


			case State.playing:
				camMovement.enabled = true;

				SetVisibility (true, KDPanel, playerPanel, minimap);
				SetVisibility (true, scoreboard);

				matchTimer.toAppend = " / " + Utils.FloatToTime (GameServer.Instance.timeLimit);
				matchTimer.StartIt (GameServer.Instance.timeLimit);
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
				SetVisibility (false, respawn, scoreboard, controls, quitConfirmation, credits, help);
				SetVisibility (true, matchOver, darkOverlay);

				SoundManager.Instance.PlayClip (SoundManager.Instance.matchOverSound);

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
		quitConfirmation= Utils.childWithName (transform, "Quit Confirmation");
		credits			= Utils.childWithName (transform, "Credits");
		help 			= Utils.childWithName (transform, "Help");

		matchTimer 		= Utils.childWithName (scoreboard, "Match Countdown").GetComponent <Countdown> ();
		Scoreboard.Instance.respawn = respawn;

		state = State.loading;
	}

	void Start ()
	{
		cameraZoom = cameraZoom;
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
		ClearPopup ();
		shownFrame = controls;
		SetVisibility (value, controls);
	}

	public void SetQuitVisibility (bool value)
	{
		ClearPopup ();
		shownFrame = quitConfirmation;
		SetVisibility (value, quitConfirmation);
	}

	public void SetCreditsVisibility (bool value)
	{
		ClearPopup ();
		shownFrame = credits;
		SetVisibility (value, credits);
	}

	public void SetHelpVisibility (bool value)
	{
		ClearPopup ();
		shownFrame = help;
		SetVisibility (value, help);
	}

	public bool ClearPopup ()
	{
		if (shownFrame != null) {
			SetVisibility (false, shownFrame);
			shownFrame = null;
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
		if (scroll != 0)
			cameraZoom += scroll * CAM_ZOOM_STEP;
	}

	public void Quit ()
	{
		Application.Quit ();
	}
}
