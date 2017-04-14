using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContactEffect : TimedEffect {

	ContactDamageComponent contactDamageComp;


 	/// <summary>
	/// called by any card to specify the actor 
	/// using this card
	/// </summary>
	/// <param name="actorUsingEffect"></param>
	/// <param name="maxDamageInterval_sec"></param>
	/// <param name="damageAmount"></param>
	public DamageOnContactEffect(Actor actorUsingEffect,float maxDamageInterval_sec,float damageAmount) {
 		contactDamageComp = actorUsingEffect.gameObject.AddComponent<ContactDamageComponent>();
		maxDamageIntervel_seconds = maxDamageInterval_sec;
		damageIntervel_seconds = 0; //ready on effect added 
	}

	public override void applyEffect(Actor applyTo) {
		//effect is alwayes active
	}

	private float maxDamageIntervel_seconds;
	private float damageIntervel_seconds;
	public override bool incrmentTimer(float time_seconds) {
		damageIntervel_seconds -= time_seconds;
		return false;
	}

	public override void removeEffect() {
		GameObject.Destroy(contactDamageComp);
	}
}
