using UnityEngine;
using System.Collections;
using System;

public class UIManager : Singleton<UIManager> 
{
	public GameObject[] togglable;
	public GameObject loadingGraphic;

	void Awake ()
	{
		SetVisibility (false);
	}

	public void SetVisibility (bool value)
	{
		Array.ForEach (togglable, t => t.SetActive (value));
		loadingGraphic.SetActive (!value);
	}
}
