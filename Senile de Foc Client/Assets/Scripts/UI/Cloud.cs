using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour 
{
	public float speed;
	Vector2 pos;

	void Start ()
	{
		pos = transform.localPosition;
	}

	void Update ()
	{
		pos.x += speed * Time.deltaTime;

		if (pos.x > CloudManager.Instance.maxRight)
			pos.x = CloudManager.Instance.maxLeft;

		transform.localPosition = pos;
	}
}
