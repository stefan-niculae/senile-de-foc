using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform player;

	// Distance the player can move before the camera catches up
	public float margin;
	public float speed;
	public float trehshold;

	Vector3 target;
	float lastMoved = float.MaxValue;

	void Start ()
	{
		target = transform.position;
	}

	void Update ()
	{
		// Ignore the margin if the camera moved recently
//		if (Mathf.Abs (transform.position.x - player.position.x) > margin || Time.time - lastMoved < trehshold) {
//			target.x = player.position.x;
//			lastMoved = Time.time;
//		}
//				
//		if (Mathf.Abs (transform.position.y - player.position.y) > margin || Time.time - lastMoved < trehshold) {
//			target.y = player.position.y;
//			lastMoved = Time.time;
//		}
//	
//		transform.localPosition = Vector3.Lerp (
//			transform.localPosition,
//			target,
//			speed);

		transform.position = new Vector3 (player.position.x, player.position.y, transform.position.z);
	}


}
