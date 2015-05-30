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
		tankMass, 			// TODO maybe add variable tank mass
		forwardSpeed,
		backwardSpeed,
		rotationSpeed,
		explosionDamage, 	// in 0-1 health percentage
		explosionRadius, 	// TODO: maybe scale particle with explosion damage
		projectileSpeed,	
		projectileMass, 	// TODO maybe add variable projectile mass
		fireRate, 			// in seconds
		barrelSpeed; 	

	
	public enum SpecialPower { mine, damageWave, DoTWave, pushWave };
}
