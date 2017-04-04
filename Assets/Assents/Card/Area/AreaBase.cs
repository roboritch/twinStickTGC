using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBase : Card {
	#region Initalization of static members
	static AreaBase() { } //insures these values are overwriten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initalization of parent vars
	// sprite is done via the unity inspecter by 
	// clicking on this script in the project assets window

	protected string getIconPath() {
		return GetType().Name + "/";
	}

	public AreaBase() {
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
	}
	#endregion

	/// <summary>
	/// can return null if loading prefab fails
	/// </summary>
	/// <returns></returns>
	protected DamageAreaControls createDamageAreas() {
		GameObject AreaPrefab = CardPrefabResorceLoader.Instance.loadPrefab(getAreaPrefabPath());
		if(AreaPrefab == null) {
			Debug.LogError("assent Not found!" + StackTraceUtility.ExtractStackTrace());
			return null;
		}
        return UnityEngine.Object.Instantiate(AreaPrefab).GetComponent<DamageAreaControls>();
	}
	
	protected string getAreaPrefabPath() {
		return this.GetType().Name + "/" + "DamageArea";
	}

	protected bool actorSelectingArea = false;
	protected DamageAreaControls currentAreas;
    protected void giveUserAreaSelector(Actor cardUser) {
		currentAreas = createDamageAreas();
		currentAreas.setAimLocationCallbacks(cardUser.transform, cardUser.getActorAimCallback(), DamageAreaControls.LocationUpdateType.SetDistanceFromLocation);
	}

	protected void activateDamageArea(Actor cardUser) {
		if(currentAreas == null) {
			Debug.LogWarning("no damage area to activate!");
			return;
		}

		currentAreas.updateAreasDamageAmounts(cardUser, true);
		currentAreas.startDamageCountdowns();
		currentAreas.disconectAim();
		currentAreas = null; //detatch the area from this card
	}

	protected void destroyDamageArea() {
		if(currentAreas != null)
			currentAreas.destroyAreas();
	}

	#region overide methods
	public override void cacheResorces() {
		CardPrefabResorceLoader.Instance.cashePrefab(getAreaPrefabPath());
	}

	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		if (actorSelectingArea == false) {
			actorSelectingArea = true;
			giveUserAreaSelector(cardUser);
			return false;
		} else {
			activateDamageArea(cardUser);
			return true;
		}
	}

	public override void destroyCard() {
		destroyDamageArea();
	}
	#endregion
}

public struct areaDamageStats {
	
	public areaDamageStats(string name, float timeToDamage_seconds, float damage) {
		prefabName = name;
		this.timeToDamage_seconds = timeToDamage_seconds;
		this.damage = damage;
	}

	public string prefabName;
	public float timeToDamage_seconds;
	public float damage;
}
