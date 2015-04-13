using UnityEngine;
using System.Collections;

public class TankHealth : MonoBehaviour 
{
	public Transform bar;
	public float fullLength;
	public GameObject explosionPrefab;

	float _amount;
	public float amount
	{
		get
		{
			return _amount;
		}

		set 
		{
			_amount = Mathf.Clamp (value, 0, 100);

			var scale = bar.localScale;
			scale.x = _amount / 100f * fullLength;
			bar.localScale = scale;

			if (_amount == 0 && !alreadyExploded) 
				Die ();
		}
	}

	bool alreadyExploded = false;

	void Start ()
	{
		amount = 100;
	}

	void Die ()
	{
		SpawnExplosion ();
	}

	void SpawnExplosion ()
	{
		alreadyExploded = true;

		GameObject explosion = Instantiate (
			explosionPrefab,
			transform.position,
			Quaternion.identity) as GameObject;
		explosion.transform.parent = GameObject.Find ("Explosions").transform;
	}
}
