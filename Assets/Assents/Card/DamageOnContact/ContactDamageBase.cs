using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageBase : Card {
	#region Initialization of static members
	static ContactDamageBase() { } //insures these values are overwritten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initialization of parent vars
	// sprite is done via the unity inspector by 
	// clicking on this script in the project assets window

	protected string getIconPath() {
		return GetType().Name + "/";
	}

	public ContactDamageBase() {
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
	}
	#endregion
	
	public override void cacheResorces() {
		//no prefabs to cache
	}

	public override void destroyCard() {
		//nothing to do
	}

	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		cardUser.effects.addEffect(new DamageOnContactEffect(cardUser,1f,2f));
		return true;
	}
}
