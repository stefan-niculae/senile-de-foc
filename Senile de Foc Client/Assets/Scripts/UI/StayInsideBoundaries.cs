using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class StayInsideBoundaries : MonoBehaviour 
{
	Vector3 origLocalPos;
	Transform parent;

	float
		maxTop,
		maxBot,
		maxLeft,
		maxRight;

	void Awake ()
	{
		parent = transform.parent;
		origLocalPos = transform.localPosition;
	}

	void Start ()
	{
		Vector2 maxExtents = Vector2.zero;

		// Get the maximum X and Y of each sprite renderer bounding box in the children
		GetComponentsInChildren <SpriteRenderer> ().Select (
			rend => rend.bounds.extents).ToList ().ForEach (
	            ext => 
	            {
					maxExtents.x = ext.x > maxExtents.x ? ext.x : maxExtents.x;
					maxExtents.y = ext.y > maxExtents.y ? ext.y : maxExtents.y;
			    });

		Utils.ComputeBoundaries (maxExtents, ref maxTop, ref maxBot, ref maxLeft, ref maxRight);
	}
	
	void Update ()
	{
		var unclampedPos = parent.position + origLocalPos; // where the object would normaly be
		var pos = transform.position; // the result

		pos.y = Mathf.Clamp (unclampedPos.y, maxBot, maxTop);
		pos.x = Mathf.Clamp (unclampedPos.x, maxLeft, maxRight);

		transform.position = pos;
	}
}
