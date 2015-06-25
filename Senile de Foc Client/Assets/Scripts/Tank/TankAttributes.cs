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
//		tankMass, 			// TODO (?) scalable tank mass
		forwardSpeed,
		backwardSpeed,
		rotationSpeed,
		explosionDamage, 	// in 0-1 health percentage
		explosionRadius, 	// TODO (?) scale particle size with explosion damage
		projectileSpeed,	
//		projectileMass, 	// TODO (?) scalable projectile mass
		fireRate, 			// in seconds
		barrelSpeed; 	

	
	public enum SpecialPower { mine, damageWave, DoTWave, pushWave };
}
