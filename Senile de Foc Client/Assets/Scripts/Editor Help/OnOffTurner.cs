using UnityEngine;
using System.Collections;

public class OnOffTurner : MonoBehaviour 
{
	public GameObject[] shouldBeOn;
	public GameObject[] shouldBeOff;

	void Awake ()
	{
		foreach (var go in shouldBeOn)
			go.SetActive (true);
		foreach (var go in shouldBeOff)
			go.SetActive (false);
	}
}
