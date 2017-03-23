﻿using System.Collections;
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
	private float damageAmount = 0f;


	/// <summary>
	/// destoryes the projectile
	/// </summary>
	protected void destroyProjectile() {
		UnityExtentionMethods.destoryAllChildren(transform);
	}

	protected void damageCollObject(Transform trans) {
		IDamageable hitObject = trans.GetComponent<IDamageable>();
		if (hitObject != null) {
			hitObject.takeDamage(damageAmount);
		}
	}

	public void setDamage(float damage) {
		damageAmount = damage;
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
	#endregion
}
