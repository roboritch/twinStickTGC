using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBase : Card {
	#region Initialization of static members
	static AreaBase() { } //insures these values are overwritten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initialization of parent vars
	// sprite is done via the unity inspector by 
	// clicking on this script in the project assets window

	public AreaBase() : base() {
		
	}
	#endregion

	/// <summary>
	/// can return null if loading prefab fails
	/// </summary>
	/// <returns></returns>
	protected DamageAreaControls createDamageAreas() {
		GameObject AreaPrefab = PrefabResorceLoader.Instance.loadPrefab(getAreaPrefabPath());
		if(AreaPrefab == null) {
			Debug.LogError("assent Not found!" + StackTraceUtility.ExtractStackTrace());
			return null;
		}
        return UnityEngine.Object.Instantiate(AreaPrefab).GetComponent<DamageAreaControls>();
	}
	
	protected string getAreaPrefabPath() {
		return getCardResorceFolderPath() + "DamageArea";
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
		currentAreas = null; //detach the area from this card
	}

	protected void destroyDamageArea() {
		if(currentAreas != null)
			currentAreas.destroyAreas();
	}

	#region override methods
	public override void cacheResorces() {
		base.cacheResorces();
		PrefabResorceLoader.Instance.cashePrefab(getAreaPrefabPath());
		PrefabResorceLoader.Instance.cashePrefab(getCardResorceFolderPath() + "AnvilAnimation");
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
