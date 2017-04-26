using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBase : Card {
	#region Initialization of static members
	static ProjectileWeaponBase() { } //insures these values are overwritten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initialization of parent vars

	public ProjectileWeaponBase() {
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
	}

	#endregion
	// basic vars
	protected float baseDamage = 5f;
	protected float baseProjectileSpeed = 5f;
	protected DamageTypes damageType = DamageTypes.phyisical_pearcing;

	#region Basic Methods
	protected float getProjectileSpeed(Actor cardUser) {
		return cardUser.effects.getNewProjectileSpeed(baseProjectileSpeed);
	}

	protected float getProjectileDamage(Actor actor) {
		return actor.effects.modifyDamage(baseDamage,damageType,true);
	}
	#endregion

	//These vars should be reused
	protected float projectileDistanceFromUser = .3f;
	protected void fireGun(Actor cardUser) {
		Vector2 userPosition = cardUser.get2dPostion(); //this can be changed to a muzzle location
		Vector2 projectileSize = new Vector2(0.4f, 0.4f); 
		Vector2 aimVectorFromUser = cardUser.getNormalizedAim(userPosition);

		//setting projectile properties
		ProjectileBase projectile = instantiateProjectile(0);
		projectile.transform.position = userPosition + aimVectorFromUser * projectileDistanceFromUser; //start projectile a little ways off of the user
		projectile.setVolocity(aimVectorFromUser * getProjectileSpeed(cardUser));
		projectile.setDamage(getProjectileDamage(cardUser),damageType,cardUser.Team);
		projectile.setProjectileColor(Color.yellow);
		projectile.setIgnoredColliders(new Collider2D[] { cardUser.collider });
	}

	protected ProjectileBase instantiateProjectile(int projectileType) {
		GameObject projectile = CardPrefabResorceLoader.Instance.loadPrefab(getProjectilePath(0));
        if (projectile == null) { //debug helper null check
			Debug.LogError("projectile prefab null, check resource folder\n" + this.GetType().Name + "/" + projectilePrefabInformation[projectileType].prefabName);
			return null;
		}
		projectile = UnityEngine.Object.Instantiate(projectile);
		ProjectileBase instantiatedProjectile = projectile.GetComponent<ProjectileBase>(); 
		return instantiatedProjectile;
	}

	/// <summary>
	/// the names of all your projectile prefabs
	/// </summary>
	protected projectileStats[] projectilePrefabInformation = { new projectileStats("projectile",5f,5f) };

	protected string getProjectilePath(int prefabIndex) {
		return this.GetType().Name + "/" + projectilePrefabInformation[prefabIndex].prefabName;
	}
		
	#region override vars
	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}
	
	public override bool useCard(Actor cardUser) {
		fireGun(cardUser);
		return true;
	}

	public override void cacheResorces() {
		for (int i = 0; i < projectilePrefabInformation.Length; i++) {
			CardPrefabResorceLoader.Instance.cashePrefab(getProjectilePath(i));
		}
	}

	public override void destroyCard() {
		//nothing needs to be done
	}
	#endregion
}

public struct projectileStats {
	public projectileStats(string name,float speed,float damage) {
		prefabName = name;
		this.speed = speed;
		this.damage = damage;
	}

	public string prefabName;
	public float speed;
	public float damage;	
}
