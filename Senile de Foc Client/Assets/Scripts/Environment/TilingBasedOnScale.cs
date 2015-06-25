using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TilingBasedOnScale : MonoBehaviour 
{
	Renderer rend = null;

	void Update ()
	{
		// The script shouldn't run in the game
		if (Application.isPlaying)
			return;

		if (rend == null)
			rend = GetComponent <Renderer> ();


		SnapScale ();
		SnapPos ();

		// Set the texture scale to the transform scale
		rend.sharedMaterial.mainTextureScale = 
			new Vector2 (transform.localScale.x, transform.localScale.z);
	}
	
	void SnapScale ()
	{
		// Ceil the x and y values of the scale
		Vector3 scale = transform.localScale;
		scale.x = Mathf.Ceil (scale.x);
		scale.y = Mathf.Ceil (scale.y);
		scale.z = Mathf.Ceil (scale.z);
		
		transform.localScale = scale;
	}
	
	void SnapPos ()
	{
		Vector3 pos = transform.localPosition;
		pos.x = Mathf.Ceil (pos.x);
		pos.y = Mathf.Ceil (pos.y);
		
		transform.localPosition = pos;
	}
}
