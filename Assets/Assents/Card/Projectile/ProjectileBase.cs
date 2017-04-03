using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// to use properly 
/// base.Start()
/// must be called by there repective MonoBehaviour methods
/// base.OnCollisionEnter2D(coll) must be chenged to fit behaviour wanted
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProjectileBase : MonoBehaviour {

	protected new Collider2D collider;
	protected new Rigidbody2D rigidbody;
	protected float damageAmount = 0f;
	protected DamageTypes damageType;

	/// <summary>
	/// destoryes the projectile
	/// </summary>
	protected void destroyProjectile() {
		UnityExtentionMethods.destoryAllChildren(transform);
	}

	protected void damageCollObject(Transform trans) {
		IDamageable hitObject = trans.GetComponent<IDamageable>();
		if (hitObject != null) {
			hitObject.takeDamage(damageAmount,damageType);
		}
	}

	public void setDamage(float damage, DamageTypes damageType) {
		damageAmount = damage;
		this.damageType = damageType;
	}

	public void setVolocity(Vector2 velocity) {
		rigidbody.velocity = velocity;
	}

	private SpriteRenderer sprite;
	public void setProjectileColor(Color color) {
		sprite.color = color;
	}


    public void setFireingPlayer(Collider2D coll) {
		Physics2D.IgnoreCollision(collider, coll, true);
	}

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

	#region mono methods
	void Awake() {
		collider = GetComponent<Collider2D>();
		rigidbody = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// called when this projectile hits somthing
	/// </summary>
	/// <param name="coll">colision</param>
	void OnCollisionEnter2D(Collision2D coll) {
		damageCollObject(coll.transform);
		destroyProjectile();
	}

	void Update() {
		checkLifetime();
	}
	#endregion
}
