using UnityEngine;
using System.Collections;

public class DestroyableBarrel : Damagable
{
	Vector3 initialPos;
	const float REAPPARITION_DURATION = 2;
	SpriteRenderer spriteRenderer;

	public override void OnAwake ()
	{
		maxHp = 25;
		respawnTime = 5;

		spriteRenderer = GetComponent <SpriteRenderer> ();

		damaged = new ThresholdParticle [1];
		damaged [0] = new ThresholdParticle (maxHp / 2f, Utils.childWithName (transform, "Damaged Barrel"));

		minWait = 4;
		maxWait = 6;

		initialPos = transform.position;
	}

	public override void OnStart ()
	{
		SetTransparency (0);
	}

	public override void OnUpdate ()
	{ }

	public override void OnTakingDamage (TankInfo source)
	{ }

	public override void OnDeath (TankInfo source)
	{
		source.playerInfo.stats.barrels++;
		GameServer.Instance.SendStatsUpdate (source.playerInfo.orderNumber, source.playerInfo.stats);
		source.ShowStatsRecap ();
	}

	public override void OnZeroHealth ()
	{ 
		SetTransparency (0);
	}

	public override void OnRespawn (bool firstTime)
	{
		transform.position = initialPos;
		spawnTime = Time.time;
	}

	float spawnTime;
	void Update ()
	{
		float coeff = (Time.time - spawnTime) / REAPPARITION_DURATION;
		if (coeff <= 1)
			SetTransparency (coeff);
	}

	void SetTransparency (float amount)
	{
		var col = spriteRenderer.color;
		col.a = amount;
		spriteRenderer.color = col;
	}
}
