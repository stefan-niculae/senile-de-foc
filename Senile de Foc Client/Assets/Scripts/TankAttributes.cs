using UnityEngine;
using System.Collections;

public class TankAttributes : MonoBehaviour 
{
	public class AttributeRate
	{
		float baseValue;
		float incrValue;

		public AttributeRate (float baseValue, float incrValue)
		{
			this.baseValue = baseValue;
			this.incrValue = incrValue;
		}

		public float applyLevel (float level)
		{
			return baseValue + level * incrValue;
		}
	}

	static readonly AttributeRate

		DAMAGE_ABSORBTION = new AttributeRate (0f, // in 0-1 percentage
		                       			   	   .05f),

		FORWARD_SPEED = new AttributeRate (35000f,
		                          		    2000f),
		BACKWARD_SPEED = new AttributeRate (28000,
		                                     1000f),
		ROTATION_SPEED = new AttributeRate (180f,
		                            		8f),

		EXPLOSION_DAMAGE = new AttributeRate (1f, // in 0-100 health percentage
		                                  	  .9f),
		EXPLOSION_RADIUS = new AttributeRate (.02f, // TODO: scale particle with this
		                                  	  .13f),
			
		BULLET_SPEED = new AttributeRate (1000f,
		                                   100f),
		FIRE_COOLDOWN = new AttributeRate (.5f, // in seconds
		                                   -.04f),
		BARREL_SPEED = new AttributeRate (2.5f,
		                                  -.32f);

	// TODO: set these in the editor
	public GameObject
		missileFireParticles,
		bulletFireParticles;

	public GameObject
		heavyMissilePrefab,
		angryMissilePrefab,
		calmBulletPrefab,
		sneakyBulletPrefab;

	public GameObject
		missileExplosionPrefab,
		bulletExplosionPrefab;

	public class Attributes
	{
		// Shown to the player
		public int armor;
		public int movement;
		public int damage;
		public int firerate;

		// Hidden from the player
		public int bounces;
		public SpecialPower special;

		public GameObject fireParticles;
		public GameObject projectilePrefab;
		public GameObject explosionPrefab;

		// Computed values
		public float damageAbsorbtion 
		{ get { return DAMAGE_ABSORBTION.applyLevel (armor); } }
		
		public float forwardSpeed
		{ get { return FORWARD_SPEED.applyLevel (movement); } }
		public float backwardSpeed
		{ get { return BACKWARD_SPEED.applyLevel (movement); } }
		public float rotationSpeed
		{ get { return ROTATION_SPEED.applyLevel (movement); } }
		
		public float explosionDamage
		{ get { return EXPLOSION_DAMAGE.applyLevel (damage); } }
		public float explosionRadius
		{ get { return EXPLOSION_RADIUS.applyLevel (damage); } }

		public float fireCooldown
		{ get { return FIRE_COOLDOWN.applyLevel (firerate); } }
		public float bulletSpeed
		{ get { return BULLET_SPEED.applyLevel (firerate); } }
		public float barrelSpeed
		{ get { return BARREL_SPEED.applyLevel (firerate); } }

		public Attributes (int armor, int movement, int damage, int firerate, 
		                   int bounces, SpecialPower special,
		                   GameObject fireParticles, GameObject projectilePrefab, GameObject explosionPrefab)
		{
			this.armor = armor;
			this.movement = movement;
			this.damage = damage;
			this.firerate = firerate;

			this.bounces = bounces;
			this.special = special;
		}
	}

	public enum SpecialPower { mine, damageWave, DoTWave, pushWave };

	public static readonly Attributes

		HEAVY =	new Attributes (armor: 		10,
		                        movement: 	1,
		                        damage: 	10,
		                        firerate: 	1,

		                        bounces: 	0,
		                        special: 	SpecialPower.pushWave,

		                        fireParticles: 		OR.missileFireParticles,
		                        projectilePrefab: 	OR.heavyMissilePrefab,
		                        explosionPrefab:	OR.missileExplosionPrefab),

		ANGRY = new Attributes (armor: 		7,
		                        movement: 	4,
		                        damage: 	7,
		                        firerate: 	4,

		                        bounces: 	1,
		                        special: 	SpecialPower.damageWave,
		                        
		                        fireParticles: 		OR.missileFireParticles,
		                        projectilePrefab: 	OR.angryMissilePrefab,
		                        explosionPrefab:	OR.missileExplosionPrefab),

		CALM = 	new Attributes (armor: 		4,
		                        movement: 	7,
		                        damage: 	4,
		                        firerate: 	7,

		                        bounces: 	2,
		                        special: 	SpecialPower.DoTWave,
		                        
		                        fireParticles: 		OR.bulletFireParticles,
		                        projectilePrefab: 	OR.calmBulletPrefab,
		                        explosionPrefab:	OR.bulletExplosionPrefab),

		SNEAKY =new Attributes (armor: 		1,
		                        movement: 	10,
		                        damage: 	1,
		                        firerate: 	10,

		                        bounces: 	0,
		                        special: 	SpecialPower.mine,
		                        
		                        fireParticles: 		OR.bulletFireParticles,
		                        projectilePrefab: 	OR.sneakyBulletPrefab,
		                        explosionPrefab:	OR.bulletExplosionPrefab);

	static TankAttributes OR;

	void Awake ()
	{
		// We use this object reference because unity editor can not set public static variables
		OR = this;
	}
}
