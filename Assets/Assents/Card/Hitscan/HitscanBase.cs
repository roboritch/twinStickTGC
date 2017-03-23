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

	protected string getIconPath() {
		return GetType().Name + "/";
    }

	public HitscanBase() { 
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
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

		Vector3 userLocation = cardUser.transform.position;

		RaycastHit2D[] hits = new RaycastHit2D[1]; 
		int hitNumb = cardUser.collider.Raycast(aimLocation-(Vector2)userLocation, hits,20f); //TODO change aim location to relative direction
		if(hitNumb == 0) {
			displayRay(userLocation, Vector3.LerpUnclamped(userLocation, aimLocation, 10f));
			return null;
		}
		IDamageable hitObject = hits[0].transform.GetComponent<IDamageable>();
		displayRay(userLocation, hits[0].point);
		return hitObject;
	}
	
	protected void displayRay(Vector3 startLocation,Vector3 endLocation) {
		GameObject beamGO = CardPrefabResorceLoader.Instance.loadPrefab(getBeamPath(0));
		BeamAnimation beam = UnityEngine.Object.Instantiate(beamGO).GetComponent<BeamAnimation>();
		beam.setBeamLength(startLocation, endLocation);
		beam.setBeamColor(Color.red);
		beam.startBeamAnimation(2f);
		
	}

	protected string[] hitscanFireAnimationPaths = { "beam" };
	protected string getBeamPath(int animationIndex) {
		return this.GetType().Name + "/" + hitscanFireAnimationPaths[animationIndex];
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
		for (int i = 0; i < hitscanFireAnimationPaths.Length; i++) {
			CardPrefabResorceLoader.Instance.cashePrefab(getBeamPath(i));
		}
	}
	#endregion
}
