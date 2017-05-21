using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_MeleeWeapon : Card {
	#region Initialization of static members
	static Card_MeleeWeapon() { } //insures these values are overwritten properly
	#endregion

	#region initialization of parent vars
	// sprite is done via the unity inspector by 
	// clicking on this script in the project assets window

	public Card_MeleeWeapon() : base() {

	}
	#endregion





	#region override methods

	public override void cacheResorces() {
		base.cacheResorces();


	}

	public override void destroyCard() {
		throw new NotImplementedException();
	}

	public override bool useCard(Actor cardUser) {
		throw new NotImplementedException();
	}
	#endregion
}
