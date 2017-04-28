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
	protected float damageAmount = 0f;
	protected DamageTypes damageType;
	protected DamageSources sourceTeam;
	protected Effect[] effectsApplyedOnContact;


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
		if (hitObject != null) {
			if(hitObject.ignoreDamage(sourceTeam, damageType)) {
				return;
			}
			hitObject.takeDamage(damageAmount,damageType,sourceTeam);
			destroyProjectile();
		} else {
			//the object is not considered in calculations as it is not damageable
			/*WARNING turning non damageable objects into damageable ones 
			will ignore this objects collider*/
			setIgnoredColliders(new Collider2D[] { colider });
		}
	}

	#region Specify projectile properties
	public void setDamage(float damage, DamageTypes damageType,DamageSources team) {
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
	/// ignore future collisions with Colliders specified 
	/// </summary>
	/// <param name="coll">player firing the projectile</param>
	public void setIgnoredColliders(Collider2D[] colliders) {
		foreach (Collider2D coll in colliders) {
			Physics2D.IgnoreCollision(collider, coll, true);
		}
	}

	#region Projectile lifetime
	private float projectileTimeLeft_seconds = 10f; //this should be fine for all but the slowest projectiles
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
}

public struct projectileStats {
	public projectileStats(string name, float speed, float damage,DamageTypes damageType) {
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
