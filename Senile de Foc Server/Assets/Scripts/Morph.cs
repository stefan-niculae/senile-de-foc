using UnityEngine;
using System.Collections;

public class Morph : MonoBehaviour 
{
	void Start ()
	{
		var posRen = gameObject.AddComponent <SpriteRenderer> ();
		posRen.sprite = SpriteReferences.instance.positionMarker;

		foreach (Transform child in transform)
			if (child != transform) {
				var ptrRen = child.gameObject.AddComponent <SpriteRenderer> ();
				ptrRen.sprite = SpriteReferences.instance.pointerMarker;
				child.transform.localPosition = new Vector3 (0, 0, -1);
			}
	}
}
