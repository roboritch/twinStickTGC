using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// effected actor takes 2 damage every 2 seconds
/// </summary>
public class DamageOverTimeEffect : TimedEffect {
	public DamageOverTimeEffect() : base(false, false) {
		effectType = EffectTypes.damageOverTime;
		maxTime_seconds = 2f;
	}

	public override void apply(Actor applyTo) {
		//this effect does not discriminate between friend and foe when applied to an actor
		applyTo.takeDamage(2, DamageTypes.heat, DamageSources.none);
	}

	public override void initalize(Actor actor) {
		throw new NotImplementedException();
	}

	public override void removeEffect() {
		//no removal requirements
	}

	public override void setCreator(Actor creator) {
		throw new NotImplementedException();
	}



	public override EffectProperties getEffectPropertiesStructure(bool forGUI) {
		throw new NotImplementedException();
	}

	public override void setEffectProperties(EffectProperties properties) {
		EffectProperties props = new EffectProperties();

	}
}
