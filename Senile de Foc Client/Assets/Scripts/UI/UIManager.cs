using UnityEngine;
using System.Collections;
using System;

public class UIManager : Singleton<UIManager> 
{
	public GameObject[] togglable;
	public GameObject loadingGraphic;
	CameraMovement camMovement;

	GameObject playerPanel;

	void Awake ()
	{
		camMovement = GameObject.Find ("Main Camera").GetComponent <CameraMovement> ();
		playerPanel = Utils.childWithName (transform, "Controlled Player Panel").gameObject;
		SetVisibility (false);
	}

	public void SetVisibility (bool value)
	{
		camMovement.enabled = value;
		Vector3 pos = camMovement.transform.position;
		if (!value) {
			pos.x = loadingGraphic.transform.position.x;
			pos.y = loadingGraphic.transform.position.y;
		}
		else {
			pos.x = 0;
			pos.y = 0;
		}
		Camera.main.transform.position = pos;


		Array.ForEach (togglable, t => t.SetActive (value));
		loadingGraphic.SetActive (!value);
	}

	public void SetPlayerPanelVisibility (bool value)
	{
		playerPanel.SetActive (value);
	}
}
