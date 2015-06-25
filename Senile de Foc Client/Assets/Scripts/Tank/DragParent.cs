using UnityEngine;
using System.Collections;

public class DragParent : MonoBehaviour 
{
	public Transform parent;

	void Update () 
	{
		if (transform.localPosition != Vector3.zero) {
			parent.position += transform.localPosition;
			transform.localPosition = Vector3.zero;
		}

		if (transform.localRotation != Quaternion.identity) {
			Vector3 childRot = transform.localRotation.eulerAngles;
			Vector3 parentRot = parent.rotation.eulerAngles;
			parent.rotation = Quaternion.Euler (parentRot + childRot);
			transform.localRotation = Quaternion.identity;
		}
	}
}
