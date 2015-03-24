using UnityEngine;
using System.Collections;

public class TankBullet : MonoBehaviour 
{
	public float speed;

	Rigidbody2D body;

	void Awake ()
	{
		body = GetComponent <Rigidbody2D> ();
	}

	public void Launch (Vector2 direction)
	{
		body.AddForce (speed * direction);
	}
}
