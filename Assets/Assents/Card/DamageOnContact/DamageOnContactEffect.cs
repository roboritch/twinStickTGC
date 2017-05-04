using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContactEffect : Effect {
	public DamageOnContactEffect() { }

	private ContactDamageComponent contactDamageComp;

	#region effect properties
	protected readonly int numberOfEffectProperties = 2;
	//defaults are set to prevent errors in the event
	//main properties aren't set properly
	private float maxDamageInterval_sec = 10f; 
	//test
	private float damageAmount = 0;
	#endregion

	/// <summary>
	/// called by any card to specify the actor 
	/// using this card
	/// </summary>
	/// <param name="actorUsingEffect"></param>
	/// <param name="maxDamageInterval_sec"></param>
	/// <param name="damageAmount"></param>
	public DamageOnContactEffect(Actor actorUsingEffect) {
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

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public override EffectProperties getEffectPropertiesStructure(bool forGUI) {
		EffectProperties properties = new EffectProperties(GetType().Name, numberOfEffectProperties, forGUI);
		if(forGUI) {
			properties.valueTypeName[0] = typeof(float).Name;
			properties.propertyName[0] = "maxDamageInterval_sec";

			properties.valueTypeName[1] = typeof(float).Name;
			properties.propertyName[1] = "damageAmount";

		}
		properties.value[0] = default(float);
		properties.value[1] = default(float);

		return properties;
	}

	public override void setEffectProperties(EffectProperties properties) {
		maxDamageInterval_sec = (float)properties.value[0];
		damageAmount = (float)properties.value[1];
	}
}
