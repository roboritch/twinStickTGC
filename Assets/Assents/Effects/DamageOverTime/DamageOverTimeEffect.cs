using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// effected actor takes 2 damage every 2 seconds
/// </summary>
public class DamageOverTimeEffect : TimedEffect {
	public DamageOverTimeEffect() : base(false, true) {
		effectType = EffectTypes.damageOverTime;
	}

	public override void apply(Actor applyTo) {
		//this effect does not discriminate between friend and foe when applied to an actor
		applyTo.takeDamage(damageAmount, damageType, DamageSources.none);
	}

	#region un-used overrides 
	public override void setCreator(Actor creator) {
		throw new NotImplementedException();
	}
	#endregion

	protected string animationName = "damageAnimation";
	public override void initalize(Actor effectedActor) {
		//TODO add on fire animation
	}
	private float timeLeft_seconds;
	public override bool incrmentTimer(float time_seconds) {
		if(timeLeft_seconds > 0) {
			timeLeft_seconds -= time_seconds;
			return false;
		} else {
			return true;
		}

	}

	public override void removeEffect() {
		//no removal requirements
	}


	#region effect properties

	protected float maxDamageInterval_seconds;
	protected float damageAmount;
	protected DamageTypes damageType;

	#endregion

	public override EffectProperties getEffectPropertiesStructure(bool forGUI) {
		EffectProperties properties = new EffectProperties(GetType().Name, 3, forGUI);
		if(forGUI) {
			properties.valueTypeName[0] = typeof(float).Name;
			properties.propertyName[0] = "maxDamageInterval_seconds";

			properties.valueTypeName[1] = typeof(float).Name;
			properties.propertyName[1] = "damageAmount";

			properties.valueTypeName[2] = typeof(DamageTypes).Name;
			properties.propertyName[2] = "damageType";

		}
		properties.value[0] = default(float).ToString();
		properties.value[1] = default(float).ToString();
		properties.value[2] = default(DamageTypes).ToString();

		return properties;
	}

	public override void setEffectProperties(EffectProperties properties) {
		maxDamageInterval_seconds = float.Parse(properties.value[0]);
		damageAmount = float.Parse(properties.value[1]);
		damageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), properties.value[2]);
	}

	public override void cacheResorces() {
		PrefabResorceLoader.Instance.cashePrefab(GetType().Name + '/' + animationName);
	}
}
