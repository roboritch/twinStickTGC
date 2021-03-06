﻿using System;
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

	public ContactDamageBase() : base(){
		
	}
	#endregion
	
	public override void cacheResorces() {
		
	}

	public override void destroyCard() {
		//nothing to do
	}


	public override bool useCard(Actor cardUser) {
		DamageOnContactEffect effect = new DamageOnContactEffect();
		//this is a legacy implementation that should be removed 
		EffectProperties properties = effect.getEffectPropertiesStructure(false);
		properties.value[0] = 1f.ToString();
		properties.value[1] = 2f.ToString();
		effect.setEffectProperties(properties);
		cardUser.effects.addEffect(effect);
		return true;
	}
}
