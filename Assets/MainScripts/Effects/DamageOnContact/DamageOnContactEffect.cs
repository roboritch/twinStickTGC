using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContactEffect : Effect {
	public DamageOnContactEffect() : base(false,true) {
		effectType = EffectTypes.damageOnContact;
	}

	private ContactDamageComponent contactDamageComp;

	#region effect properties
	protected readonly int numberOfEffectProperties = 2;
	//defaults are set to prevent errors in the event
	//main properties aren't set properly
	private float maxDamageInterval_sec = 10f; 
	//test
	private float damageAmount = 0;
	#endregion

	public override void apply(Actor applyTo) {
		//effect is constant
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

		properties.value[0] = default(float).ToString();
		properties.value[1] = default(float).ToString();

		return properties;
	}

	public override void setEffectProperties(EffectProperties properties) {
		maxDamageInterval_sec = float.Parse(properties.value[0]);
		damageAmount = float.Parse(properties.value[1]);
	}

	public override void setCreator(Actor creator) {
		throw new NotImplementedException();
	}

	public override void initalize(Actor actor) {
		contactDamageComp = actor.gameObject.AddComponent<ContactDamageComponent>();
		contactDamageComp.initDamage(actor, damageAmount, DamageTypes.physical_normal, maxDamageInterval_sec);
	}

	public override void cacheResorces() {
		GameObject animationPrefab = Resources.Load<GameObject>("ContactDamageComponent/spinning part");
	}
}
