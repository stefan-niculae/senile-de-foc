using UnityEngine;
using System.Collections;

public class TankAttributes : Singleton<TankAttributes>
{
	[System.Serializable]
	public class AttributeRate
	{
		public float baseValue;
		public float incrValue;

		public AttributeRate (float baseValue, float incrValue)
		{
			this.baseValue = baseValue;
			this.incrValue = incrValue;
		}

		public float compute (int level)
		{
			return baseValue + level * incrValue;
		}
	}

	public AttributeRate
		damageAbsorbtion, 	// in 0-1 percentage
		tankMass, 			// TODO
		forwardSpeed,
		backwardSpeed,
		rotationSpeed,
		explosionDamage, 	// in 0-1 health percentage
		explosionRadius, 	// TODO: scale particle with this (maybe...)
		projectileSpeed,
		projectileMass, 	// TODO
		fireRate, 			// in seconds
		barrelSpeed; 		// TODO switch check the backwards speed of this



	public enum SpecialPower { mine, damageWave, DoTWave, pushWave };
}

[System.Serializable]
public class Levels
{
	public int
		armor,
		moveSpeed,
		damage,
		fireRate;
	
	/**
	* Applies the stats level throughout the components
	*/ 
	public void Apply (TankHealth health, TankMovement movement, Cannon weapon, TankBarrel barrel, Projectile projectile, Explosion projectileExplosion) 
	{
		health.damageAbsorbtion = 	TankAttributes.Instance.damageAbsorbtion	.compute (armor);
		
		movement.forwardSpeed = 	TankAttributes.Instance.forwardSpeed		.compute (moveSpeed);
		movement.backwardSpeed = 	TankAttributes.Instance.backwardSpeed		.compute (moveSpeed);
		movement.rotationSpeed = 	TankAttributes.Instance.rotationSpeed		.compute (moveSpeed);
		
		weapon.fireRate = 			TankAttributes.Instance.fireRate			.compute (fireRate);
		barrel.backwardSpeed = 		TankAttributes.Instance.barrelSpeed			.compute (fireRate);
		barrel.forwardSpeed = 		barrel.backwardSpeed / 2f; // TODO: check if this is right
		projectile.speed = 			TankAttributes.Instance.projectileSpeed		.compute (fireRate);
		
		projectileExplosion.damage =TankAttributes.Instance.explosionDamage		.compute (damage);
		projectileExplosion.radius =TankAttributes.Instance.explosionRadius		.compute (damage);
	}
}
