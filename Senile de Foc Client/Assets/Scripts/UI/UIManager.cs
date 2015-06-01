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
	Transform respawn;
	Transform matchOver;

	void Awake ()
	{
		camMovement = Camera.main.GetComponent <CameraMovement> ();
		loadingGraphic = GameObject.Find ("Loading Graphic");

		hidden = Utils.childWithName (transform, "Hidden");
		shown  = Utils.childWithName (transform, "Canvas");

		KDPanel 	= Utils.childWithName (transform, "KD Panel");
		playerPanel = Utils.childWithName (transform, "Controlled Player Panel");
		minimap 	= Utils.childWithName (transform, "Minimap");
		darkOverlay = Utils.childWithName (transform, "Dark Overlay");
		scoreboard 	= Utils.childWithName (transform, "Scoreboard");
		respawn 	= Utils.childWithName (transform, "Respawn Frame");
		matchOver 	= Utils.childWithName (transform, "Match Over");


		LoadingView ();
	}

	void SetVisibility (bool visible, params Transform[] elements)
	{
		Array.ForEach (elements,
			elem => elem.SetParent (visible ? shown : hidden, false));
	}

	public void LoadingView ()
	{
		camMovement.enabled = false;
		Vector3 pos = loadingGraphic.transform.position;
		pos.z = Camera.main.transform.position.z;
		Camera.main.transform.position = pos;

		SetVisibility (false, matchOver);

		SetVisibility (false, KDPanel, playerPanel, minimap);
		SetVisibility (false, darkOverlay, scoreboard, respawn);
	}

	public void PlayingView ()
	{
		camMovement.enabled = true;

		SetVisibility (true, KDPanel, playerPanel, minimap);
		SetVisibility (true, scoreboard);
	}

	public void DeadView ()
	{
		SetVisibility (false, KDPanel, playerPanel);
		SetVisibility (true, darkOverlay, respawn);
	}

	public void AliveView ()
	{
		SetVisibility (false, darkOverlay, respawn);
		SetVisibility (true, KDPanel, playerPanel);
	}

	public void MatchOverView ()
	{
		SetVisibility (false, KDPanel, playerPanel, minimap);
		SetVisibility (false, darkOverlay, respawn, scoreboard);
		SetVisibility (true, matchOver);
	}
}
