using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBase : Card {
	#region Initalization of static members
	static ContactBase() { } //insures these values are overwriten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initalization of parent vars
	// sprite is done via the unity inspecter by 
	// clicking on this script in the project assets window

	protected string getIconPath() {
		return GetType().Name + "/";
	}

	public ContactBase() {
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
	}
	#endregion





	public override void cacheResorces() {
		throw new NotImplementedException();
	}

	public override void destroyCard() {
		throw new NotImplementedException();
	}

	public override void displayDescription(defaultTextHolder decriptionBox) {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		throw new NotImplementedException();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
