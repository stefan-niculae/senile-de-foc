using UnityEngine;
using System.Collections;

public class MovementIllustrator : MonoBehaviour 
{
	public Transform actual;
	public Transform projected;
	public Transform rotated;
	public TextMesh info;

	void Start ()
	{
		Time.timeScale = .1f;
	}

	void Update ()
	{
		actual.position = new Vector2 (Input.GetAxis ("Horizontal"), 
		                               Input.GetAxis ("Vertical"));

		float x = actual.position.x;
		float y = actual.position.y;
		projected.position = new Vector2 (x * Mathf.Sqrt (1 - y * y / 2),
		                                  y * Mathf.Sqrt (1 - x * x / 2));

		float rot = Mathf.Atan2 (projected.position.x,
		                         projected.position.y);
		rot *= Mathf.Rad2Deg;
		rot *= -1;
		rotated.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rot));	

		
		info.text = string.Format ("Actual      {0}\nProjected {1}\nRotation   {2}\nActual Magn {3}\nProj Magn {4}",
		                           (Vector2)actual.position, (Vector2)projected.position, rot, 
		                           actual.position.magnitude, projected.position.magnitude);
	}
}
