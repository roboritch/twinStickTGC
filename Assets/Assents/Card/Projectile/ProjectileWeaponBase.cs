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

	private void fireGun(Actor cardUser,ProjectileBase projectile) {

	}

	private void createProjectile(Vector2 projectileVolocity,ProjectileBase projectile) {

	}


	#region override vars
	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		throw new NotImplementedException();
	}
	#endregion
}
