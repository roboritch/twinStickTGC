using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBase : Card {
	#region Initalization of static members
	static ProjectileWeaponBase() { } //insures these values are overwriten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initalization of parent vars
	// sprite is done via the unity inspecter by 
	// clicking on this script in the project assets window

	public ProjectileWeaponBase() {
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = SpriteHolder.Instance.pistol;
	}
	#endregion
	
	#region Basic Methods
	protected float getProjectileSpeed(Actor cardUser) {
		//TODO add interface that can be found in Actor that changes projectile speed
		return 5f;
	}

	protected float getProjectileDamage() {
		//TODO add interface that can be found in Actor that changes projectile damage
		return 5f;
	}
	#endregion

	//These vars should be reused
	protected float projectileDistanceFromUser;
	protected void fireGun(Actor cardUser) {
		Vector2 userPosition = cardUser.get2dPostion(); //this can be changed to a muzzle location
		Vector2 projectileSize = new Vector2(0.4f, 0.4f); 
		Vector2 aimVectorFromUser = cardUser.getNormalizedAim(userPosition);

		//setting projectile properties
		ProjectileBase projectile = instantiateProjectile(0);
		projectile.transform.position = userPosition + aimVectorFromUser * .5f;
		projectile.setVolocity(aimVectorFromUser * getProjectileSpeed(cardUser));
		projectile.setDamage(getProjectileDamage());

	}

	protected ProjectileBase instantiateProjectile(int projectileType) {
		GameObject projectile = projectilePrefabs[projectileType];
		if(projectile == null) { //debug helper null check
			Debug.LogError("projectile prefab null, check resorce folder\n" + this.GetType().Name + "/" + projectilePrefabNames[projectileType]);
			return null;
		}
		projectile = UnityEngine.Object.Instantiate(projectile);
		ProjectileBase instantiatedProjectile = projectile.GetComponent<ProjectileBase>(); 
		return instantiatedProjectile;
	}

	/// <summary>
	/// the names of all your projectile prefabs
	/// </summary>
	protected projectileStats[] projectilePrefabNames = { new projectileStats("projectile",5f,5f) };
	protected GameObject[] projectilePrefabs;
	protected void loadProjectileResorces() {
		projectilePrefabs = new GameObject[projectilePrefabNames.Length];
		for (int i = 0; i < projectilePrefabNames.Length; i++) {
			projectilePrefabs[i] = CardPrefabResorceLoader.Instance.loadPrefab(getProjectilePath(i));
		}
	}

	protected string getProjectilePath(int projectileIndex) {
		return this.GetType().Name + "/" + projectilePrefabNames[projectileIndex];
	}
		
	#region override vars
	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		
		return true;
	}

	public override void cacheResorces() {
		for (int i = 0; i < projectilePrefabNames.Length; i++) {
			CardPrefabResorceLoader.Instance.cashePrefab(getProjectilePath(i));
		}
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