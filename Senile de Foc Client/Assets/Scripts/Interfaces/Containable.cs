using UnityEngine;
using System.Collections;

// We make the class generic so that each class that implements it gets a separate static container
public abstract class Containable <T> : MonoBehaviour
{
	static Transform container;

	protected void moveToContainer (string name)
	{
		if (container == null)
			container = GameObject.Find (name).transform;

		transform.parent = container;
	}
}
