using UnityEngine;
using System.Collections;

public class BulletExplosion : MonoBehaviour 
{
	static readonly float TIME_TO_LIVE = 5f;
	public float radius;
	public float damage;

	void Start ()
	{
		DamageAround ();
		Destroy (gameObject, TIME_TO_LIVE);
	}

	void DamageAround ()
	{
		Collider2D[] around = Physics2D.OverlapCircleAll (transform.position, radius);
		foreach (Collider2D coll in around)
			if (coll.tag == "Player")
				coll.GetComponent <TankHealth> ().amount -= damage;
	}
}
