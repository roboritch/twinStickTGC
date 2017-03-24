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

	protected DamageArea[] damageAreas;
	protected void createDamageAreas(Actor cardUser) {
		Vector2 aimDirection = cardUser.getNormalizedAim(cardUser.get2dPostion());
		UnityEngine.Object.Instantiate(CardPrefabResorceLoader.Instance.loadPrefab(getAreaPrefabPath(0))).GetComponent<DamageArea>();
	}
	
	protected areaDamageStats[] areaDamagePrefabNames = { new areaDamageStats("squair",2f,10f) };
	protected string getAreaPrefabPath(int prefabIndex) {
		return this.GetType().Name + "/" + areaDamagePrefabNames[prefabIndex];
	}

	public override void cacheResorces() {
		for (int i = 0; i < areaDamagePrefabNames.Length; i++) {
			CardPrefabResorceLoader.Instance.cashePrefab(getAreaPrefabPath(0));
		}
	}

	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		throw new NotImplementedException();
	}
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
