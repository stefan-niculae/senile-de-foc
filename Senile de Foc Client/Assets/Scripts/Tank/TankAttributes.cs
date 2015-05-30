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
		explosionRadius, 	// TODO: scale particle with this?
		projectileSpeed,
		projectileMass, 	// TODO
		fireRate, 			// in seconds
		barrelSpeed; 		// TODO switch check the backwards speed of this

	
	public enum SpecialPower { mine, damageWave, DoTWave, pushWave };
}
