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

	//These vars should be reused
	protected float projectileDistanceFromUser = .3f;

	private ProjectileBase instantiateProjectile() {
		GameObject projectile = CardPrefabResorceLoader.Instance.loadPrefab(getProjectilePath(0));
		if(projectile == null) { //debug helper null check
			Debug.LogError("projectile prefab null, check resource folder\n" + this.GetType().Name + "/" + projectilePrefabInformation[0].prefabName);
			return null;
		}
		projectile = UnityEngine.Object.Instantiate(projectile);
		ProjectileBase instantiatedProjectile = projectile.GetComponent<ProjectileBase>();
		return instantiatedProjectile;
	}

	/// <summary>
	/// the names of all your projectile prefabs
	/// </summary>
	protected projectileStats[] projectilePrefabInformation = { new projectileStats("projectile",5f,5f, DamageTypes.phyisical_pearcing) };

	protected string getProjectilePath(int prefabIndex) {
		return this.GetType().Name + "/" + projectilePrefabInformation[prefabIndex].prefabName;
	}
		
	#region override vars
	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}
	
	public override bool useCard(Actor cardUser) {
		S_ProjectileCreationHelper.projectile(cardUser, projectileDistanceFromUser,instantiateProjectile() ,projectilePrefabInformation[0]);
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
