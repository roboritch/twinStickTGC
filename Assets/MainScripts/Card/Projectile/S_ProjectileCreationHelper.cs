﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class S_ProjectileCreationHelper {


	public static void projectile(Actor cardUser, float distanceFromUser, ProjectileBase preInstateatedProjectile) {
		Vector2 userPosition = cardUser.get2dPostion(); //this can be changed to a muzzle location
		Vector2 aimVectorFromUser = cardUser.getNormalizedAim(userPosition);

		//setting projectile properties
		ProjectileBase projectile = preInstateatedProjectile;
		projectile.transform.position = userPosition + aimVectorFromUser * distanceFromUser; //start projectile a little ways off of the user
		projectile.setVolocity(aimVectorFromUser * getProjectileSpeed(cardUser, preInstateatedProjectile.getBaseSpeed()));
		//set the damage of the projectile
		projectile.setDamage(getProjectileDamage(cardUser, preInstateatedProjectile.getBaseDamage(), preInstateatedProjectile.getDamageType()), preInstateatedProjectile.getDamageType(), cardUser.Team);
		projectile.setIgnoredColliders(new Collider2D[] { cardUser.collider });
	}

	public static void projectile(Actor cardUser,float distanceFromUser, ProjectileBase preInstateatedProjectile, ProjectileStats stats) {
		Vector2 userPosition = cardUser.get2dPostion(); //this can be changed to a muzzle location
		Vector2 aimVectorFromUser = cardUser.getNormalizedAim(userPosition);

		//setting projectile properties
		ProjectileBase projectile = preInstateatedProjectile;
		projectile.transform.position = userPosition + aimVectorFromUser * distanceFromUser; //start projectile a little ways off of the user
		projectile.setVolocity(aimVectorFromUser * getProjectileSpeed(cardUser,stats.speed));
		projectile.setDamage(getProjectileDamage(cardUser, stats.damage, stats.damageType), stats.damageType, cardUser.Team);
		projectile.setProjectileColor(Color.yellow);
		projectile.setIgnoredColliders(new Collider2D [] { cardUser.collider });
	}

	#region Basic Methods
	private static float getProjectileSpeed(Actor cardUser ,float speed) {
		return cardUser.effects.getNewProjectileSpeed(speed);
	}

	private static float getProjectileDamage(Actor actor, float damage, DamageTypes damageType) {
		return actor.effects.modifyDamage(damage, damageType, true);
	}
	#endregion


}
