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

	public ProjectileWeaponBase() : base(){

	}

	#endregion

	//These vars should be reused
	protected float projectileDistanceFromUser = .3f;

	private ProjectileBase instantiateProjectile() {
		GameObject projectile = PrefabResorceLoader.Instance.loadPrefab(getProjectilePath());
		if(projectile == null) { //debug helper null check
			Debug.LogError("projectile prefab null, check resource folder\n" + getProjectilePath());
			return null;
		}
		projectile = UnityEngine.Object.Instantiate(projectile);
		ProjectileBase instantiatedProjectile = projectile.GetComponent<ProjectileBase>();
		return instantiatedProjectile;
	}

	protected string getProjectilePath() {
		return getCardResorceFolderPath() + "Projectile";
	}
		
	#region override vars
	
	public override bool useCard(Actor cardUser) {
		S_ProjectileCreationHelper.projectile(cardUser, projectileDistanceFromUser,instantiateProjectile());
		return true;
	}

	public override void cacheResorces() {
		PrefabResorceLoader.Instance.cashePrefab(getProjectilePath());
	}

	public override void destroyCard() {
		//nothing needs to be done
	}
	#endregion
}
