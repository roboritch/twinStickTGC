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

	protected DamageAreaControls createDamageAreas() {
		return UnityEngine.Object.Instantiate(CardPrefabResorceLoader.Instance.loadPrefab(getAreaPrefabPath()).GetComponent<DamageAreaControls>());
	}
	
	protected string getAreaPrefabPath() {
		return this.GetType().Name + "/" + "damage areas";
	}

	
	protected bool actorSelectingArea = false;
	protected void giveUserAreaSelector(Actor cardUser) {
		DamageAreaControls areas = createDamageAreas();
		foreach (DamageArea area in areas.damageAreas) {
			area.damageAmount 
		}	
	
	}


	#region overide methods
	public override void cacheResorces() {
		CardPrefabResorceLoader.Instance.cashePrefab(getAreaPrefabPath());
	}

	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		if(actorSelectingArea == false) {
			actorSelectingArea = true;


		}
	}

	public override void destroyCard() {
		
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
