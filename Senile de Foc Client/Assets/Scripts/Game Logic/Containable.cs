using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// We make the class generic so that each class that implements it gets a separate static container
public abstract class Containable <T> : MonoBehaviour
{
	static GameObject go;
	static Transform transf;
	static RectTransform rectTransf;

	RectTransform selfRect;

	// Remember to use this in awake in the derived classes
	protected void moveToContainer (string name)
	{
		if (go == null) {
			go = GameObject.Find (name);
			transf = go.transform;
			rectTransf = go.GetComponent <RectTransform> ();
		}

		if (rectTransf != null) {
			if (selfRect == null)
				selfRect = GetComponent <RectTransform> ();
			selfRect.SetParent (rectTransf, false);
		}

		else if (transf != null)
			transform.parent = transf;
	}
}
