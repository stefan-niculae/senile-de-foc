using UnityEngine;
using System.Collections;

public class DestroyableBarrel : MonoBehaviour 
{
	static readonly float MAX_HP = 20;
	static readonly float RESPAWN_TIME = 2f;

	static Transform explosionContainer;

	public GameObject explosionPrefab;
	ParticleSystem damagedParticles;
	float hp;

	void Awake ()
	{
		if (explosionContainer == null)
			explosionContainer = GameObject.Find ("Explosions").transform;

		damagedParticles = Utils.childWithName (transform, "Damaged Barrel").GetComponent <ParticleSystem> ();
	}

	void Start ()
	{
		hp = MAX_HP; // TODO: add smoke if barrel is damaged
	}

	void Update ()
	{
		if (hp <= MAX_HP / 2f /*&& !damagedParticles.isPlaying*/) // TODO: make this into a nice stream of smoke
			damagedParticles.Play ();
		else
			damagedParticles.Stop ();
	}

	public void TakeDamage (float damage, PlayerStats source)
	{
		hp -= damage;
		if (hp <= 0) {
			source.barrels++;
			Explode ();
			gameObject.SetActive (false);
//			StartCoroutine (WaitAndReappear (RESPAWN_TIME)); // TODO
		}
	}

	bool exploded;
	void Explode ()
	{
		if (!exploded) {
			exploded = true;

			GameObject explosion = Instantiate (
				explosionPrefab,
				transform.position,
				Quaternion.identity) as GameObject;

			explosion.transform.parent = explosionContainer;
		}
	}

	IEnumerator WaitAndReappear (float time)
	{
		yield return new WaitForSeconds (time);

		// TODO: make alpha scale from 0 to 1 here
		gameObject.SetActive (true);
		exploded = false;
		hp = MAX_HP;
		damagedParticles.Stop ();
	}

}
