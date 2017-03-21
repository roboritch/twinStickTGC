using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitscanBase : Card {
	#region Initalization of static members
	static HitscanBase() { } //insures these values are overwriten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initalization of parent vars
	// sprite is done via the unity inspecter by 
	// clicking on this script in the project assets window

	public HitscanBase() { 
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
		int hitNumb = cardUser.collider.Raycast(aimLocation-(Vector2)cardUser.transform.position, hits,20f); //TODO change aim location to relative direction
		if(hitNumb == 0) {
			return null;
		}
		IDamageable hitObject = hits[0].transform.GetComponent<IDamageable>();
		return hitObject;
	}
	

	#region override vars
	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		fireGun(cardUser);
		return true;
	}
	#endregion
}
