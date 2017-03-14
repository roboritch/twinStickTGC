using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileGun : Card {
	#region Initalization of static members
	static ProjectileGun() { } //insures these values are overwriten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initalization of parent vars
	// sprite is done via the unity inspecter by 
	// clicking on this script in the project assets window

	public ProjectileGun() { 
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = SpriteHolder.Instance.pistol;
    }
	#endregion

	private void fireGun(Actor cardUser) {
		IDamageable hitObject = raycastProjectile(cardUser);
        if (hitObject != null) {
			hitObject.takeDamage(3f);
		}
	}
	
	

	protected IDamageable raycastProjectile(Actor cardUser) {
		Vector2 aimLocation = new Vector2();
		ExecuteEvents.Execute<IGetAim>(cardUser.gameObject, null, (x, y) => x.getAim(out aimLocation));

		RaycastHit2D[] hits = new RaycastHit2D[1]; 
		int hitNumb = cardUser.collider.Raycast(aimLocation, hits,20f); //TODO change aim location to relative direction
		if(hitNumb == 0) {
			return null;
		}
		IDamageable hitObject = hits[0].transform.GetComponent<IDamageable>();
		return hitObject;
	}

	#region overrid vars
	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		fireGun(cardUser);
		return true;
	}
	#endregion
}
