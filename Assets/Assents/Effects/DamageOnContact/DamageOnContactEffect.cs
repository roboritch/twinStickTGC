using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContactEffect : Effect {

	private ContactDamageComponent contactDamageComp;
	
 	/// <summary>
	/// called by any card to specify the actor 
	/// using this card
	/// </summary>
	/// <param name="actorUsingEffect"></param>
	/// <param name="maxDamageInterval_sec"></param>
	/// <param name="damageAmount"></param>
	public DamageOnContactEffect(Actor actorUsingEffect,float maxDamageInterval_sec,float damageAmount) {
 		contactDamageComp = actorUsingEffect.gameObject.AddComponent<ContactDamageComponent>();
		contactDamageComp.initDamage(actorUsingEffect, damageAmount, DamageTypes.physical_normal, maxDamageInterval_sec);
	}

	public override void applyEffect(Actor applyTo) {
		//effect is always active
	}

	/// <summary>
	/// destroy the component attached to the object 
	/// </summary>
	public override void removeEffect() {
		UnityEngine.Object.Destroy(contactDamageComp);
	}
}
