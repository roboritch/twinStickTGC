using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// to use properly 
/// base.Start()
/// must be called by there respective MonoBehaviour methods
/// base.OnCollisionEnter2D(coll) must be changed to fit behavior wanted
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProjectileBase : MonoBehaviour {

	protected new Collider2D collider;
	protected new Rigidbody2D rigidbody;
	protected DamageSources sourceTeam;


	#region inspector properties
	[SerializeField]
	[Range(0,100f)]
	protected float projectileTimeLeft_seconds = 10f;
	[SerializeField] protected float damageAmount = 0f;
	[SerializeField] protected float speed = 1f;
	[SerializeField] protected DamageTypes damageType = DamageTypes.phyisical_pearcing;
	[SerializeField] public EffectProperties[] effectsApplyedOnContact;
	#endregion

	#region projectile creation get and set
	public float getBaseDamage() {
		return damageAmount;
	}

	public float getBaseSpeed() {
		return speed;
	}

	public DamageTypes getDamageType() {
		return damageType;
	}

	//set methods to update properties with the using actors effects
	public void setDamage(float damage, DamageTypes damageType, DamageSources team) {
		damageAmount = damage;
		this.damageType = damageType;
		sourceTeam = team;
	}

	public void setVolocity(Vector2 velocity) {
		rigidbody.velocity = velocity;
	}

	private SpriteRenderer sprite;
	public void setProjectileColor(Color color) {
		sprite.color = color;
	}
	#endregion





	/// <summary>
	/// destroys the projectile along with it's children
	/// </summary>
	protected void destroyProjectile() {
		UnityExtentionMethods.destoryAllChildren(transform);
	}

	/// <summary>
	/// check if object is damageable 
	/// </summary>
	/// <param name="colider"></param>
	protected void objectCollision(Collider2D colider) {
		IDamageable hitObject = colider.GetComponent<IDamageable>();
		if(hitObject != null) {
			if(hitObject.ignoreDamage(sourceTeam, damageType)) {
				return;
			}
			//damage hit object
			hitObject.takeDamage(damageAmount, damageType, sourceTeam);
			//apply effects to object
			if(effectsApplyedOnContact != null)
				foreach(EffectProperties effect in effectsApplyedOnContact) {
					Effect effectInsance = (Effect)System.Activator.CreateInstance(System.Type.GetType(effect.effectClassName));
					effectInsance.setEffectProperties(effect);
					hitObject.addEffect(effectInsance);
				}

			destroyProjectile();
		} else {
			//the object is not considered in calculations as it is not damageable
			/*WARNING turning non damageable objects into damageable ones 
			will ignore this objects collider*/
			setIgnoredColliders(new Collider2D[] { colider });
		}
	}


	/// <summary>
	/// ignore future collisions with Colliders specified 
	/// </summary>
	/// <param name="coll">player firing the projectile</param>
	public void setIgnoredColliders(Collider2D[] colliders) {
		foreach (Collider2D coll in colliders) {
			Physics2D.IgnoreCollision(collider, coll, true);
		}
	}

	#region Projectile lifetime
	public void setProjectileLifetime(float lifetime_seconds) {
		projectileTimeLeft_seconds = lifetime_seconds;
	}

	private void checkLifetime() {
		projectileTimeLeft_seconds -= Time.deltaTime;
		if(projectileTimeLeft_seconds <= 0f) {
			destroyProjectile();
		}
	}
	#endregion

	#region mono methods
	void Awake() {
		collider = GetComponent<Collider2D>();
		rigidbody = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// called when this projectile hits something
	/// </summary>
	/// <param name="coll">collision</param>
	void OnCollisionEnter2D(Collision2D coll) {
		objectCollision(coll.collider);
	}

	void Update() {
		checkLifetime();
	}
	#endregion

	#region extra serialization 
	//[SerializeField, HideInInspector]
	//private string[] S_effectsApplyedOnContact;

	//void ISerializationCallbackReceiver.OnBeforeSerialize() { 
	//	S_effectsApplyedOnContact = new string[effectsApplyedOnContact.Length];
	//	for(int i = 0; i < effectsApplyedOnContact.Length; i++) {
	//		SaveAndLoadJson.saveStructToString(effectsApplyedOnContact[i], out S_effectsApplyedOnContact[i]);
	//	}
	//}

	//void ISerializationCallbackReceiver.OnAfterDeserialize() {
	//	effectsApplyedOnContact = new EffectProperties[S_effectsApplyedOnContact.Length];
	//	for(int i = 0; i < S_effectsApplyedOnContact.Length; i++) {
	//		SaveAndLoadJson.loadStructToString(out effectsApplyedOnContact[i], S_effectsApplyedOnContact[i]);
	//	}
	//}
	#endregion
}

public struct ProjectileStats {
	public ProjectileStats(string name, float speed, float damage,DamageTypes damageType) {
		prefabName = name;
		this.speed = speed;
		this.damage = damage;
		this.damageType = damageType;
	}

	public string prefabName;
	public float speed;
	public float damage;
	public DamageTypes damageType;
}
