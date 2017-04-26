using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitscanBase : Card {
	#region Initialization of static members
	static HitscanBase() { } //insures these values are overwritten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initialization of parent vars

	public HitscanBase() { 
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
    }
	#endregion
	protected float baseDamage = 5f;
	protected DamageTypes damageType = DamageTypes.phyisical_diffuse;
	protected float projectileSize = 1f;
	protected float projectileDistance = 10f;

	protected void fireGun(Actor cardUser) {
		//simple raycast
		//IDamageable hitObject = raycastProjectile(cardUser);
		//advanced 
		float damageAmount = cardUser.effects.modifyDamage(baseDamage, damageType, true);
		IDamageable[] hitObjects = raycastWideProjectile(cardUser, true, damageAmount);
		if(hitObjects != null)
		for (int i = 0; i < hitObjects.Length; i++) {
			hitObjects[i].takeDamage(damageAmount,damageType,cardUser.Team);
		}
	}
	
	//TODO adv: have ray slip past corners rather than stopping when part of the ray hits an impassible target
	/// <summary>
	/// returns all IDamageble objects hit in order of distance ascending
	/// </summary>
	/// <param name="cardUser"></param>
	/// <returns></returns>
	protected IDamageable[] raycastWideProjectile(Actor cardUser,bool multiHit,float damage) {
		Vector2 aimLocation = cardUser.getAimLocation();


		Vector2 userLocation = cardUser.transform.position;
		//increase initial distance to avoid colliding with back walls
		userLocation += (aimLocation - userLocation).normalized * 0.5f;
		RaycastHit2D[] hits;
		//ordered from lowest to highest distance
		hits = Physics2D.CircleCastAll(userLocation , projectileSize*.5f, aimLocation - userLocation, projectileDistance);
		if (hits == null)
			return null;

		int hitIndex = 0;

		IDamageable[] validHitTargets = new IDamageable[hits.Length];
		RaycastHit2D lastHitTarget = new RaycastHit2D();

		for (int i = 0; i < hits.Length; i++) {
			if (!(hits[i].collider == cardUser.collider)) { // only run if not shooter
				IDamageable validCheck = hits[i].collider.GetComponent<IDamageable>();
				if (validCheck != null) {
					if (validCheck.ignoreDamage(cardUser.Team, damageType)) {
						break;
					}
					lastHitTarget = hits[i];
					validHitTargets[hitIndex++] = validCheck;
					if (validCheck.blocksDamage(damage, damageType)) { //if valid IDamagable blocks this damage stop here
						break;
					}
				}
			}
		}

		if(hitIndex == 0) {
			displayRay(userLocation, Vector3.LerpUnclamped(userLocation, aimLocation, projectileDistance),projectileSize);
			return null;
		}

		//this is will not look right if the hit target is vary small or
		//the very edge of the cast hits a target
		Vector2 displayRayEndpoint = lastHitTarget.centroid - userLocation;
		if(displayRayEndpoint == new Vector2()) {
			//set min endpoint
			displayRayEndpoint = userLocation + (aimLocation - userLocation).normalized * 0.7f;
		} else {
			displayRayEndpoint += displayRayEndpoint.normalized * projectileSize * 0.7f;
			displayRayEndpoint += userLocation;
		}

		if (multiHit) {
			Array.Resize(ref validHitTargets, hitIndex);
			displayRay(userLocation, displayRayEndpoint, projectileSize);
			return validHitTargets;
		} else {
			Array.Resize(ref validHitTargets, 1);
			displayRay(userLocation, displayRayEndpoint, projectileSize);
			return validHitTargets;
		}
	}

	protected IDamageable raycastProjectile(Actor cardUser) {
		Vector2 aimLocation = cardUser.getAimLocation();
		
		Vector2 userLocation = cardUser.transform.position;
		
		RaycastHit2D[] hits = new RaycastHit2D[1]; 
		int hitNumb = cardUser.collider.Raycast(aimLocation-userLocation, hits, projectileDistance); 
		if(hitNumb == 0) {
			displayRay(userLocation, Vector3.LerpUnclamped(userLocation, aimLocation, projectileDistance), projectileSize);
			return null;
		}
		IDamageable hitObject = hits[0].transform.GetComponent<IDamageable>();
		displayRay(userLocation, hits[0].point, projectileSize);
		return hitObject;
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="startLocation"></param>
	/// <param name="endLocation"></param>
	protected void displayRay(Vector2 startLocation, Vector2 endLocation,float rayWidth) {
		GameObject beamGO = CardPrefabResorceLoader.Instance.loadPrefab(getBeamPath(0));
		BeamAnimation beam = UnityEngine.Object.Instantiate(beamGO).GetComponent<BeamAnimation>();
		beam.setBeamLength(startLocation, endLocation);
		beam.setBeamColor(Color.red);
		beam.startBeamAnimation(2f);
	}

	protected string[] hitscanFireAnimationNames = { "beam" };
	protected string getBeamPath(int prefabIndex) {
		return this.GetType().Name + "/" + hitscanFireAnimationNames[prefabIndex];
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
		for (int i = 0; i < hitscanFireAnimationNames.Length; i++) {
			CardPrefabResorceLoader.Instance.cashePrefab(getBeamPath(i));
		}
	}

	public override void destroyCard() {
		//nothing needs to be done here
	}
	#endregion
}
