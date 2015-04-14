using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Minimap : MonoBehaviour 
{
	public float dimension = .27f;
	public float borderMargin = .008f;
	Camera cam;

	void Awake ()
	{
		cam = GetComponent <Camera> ();
	}


	void Update ()
	{
		Rect camRect = cam.rect;
		
		float ratio = (float)Screen.width / Screen.height;

		// We do it in this order because the canvas is set to follow the height first
		camRect.height = dimension;
		camRect.width = dimension / ratio;

		camRect.y = borderMargin;
		camRect.x = 1f - camRect.width - borderMargin / ratio;

		cam.rect = camRect;
	}
}
