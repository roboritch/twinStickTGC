using UnityEngine;
using System.Collections;
using EffectNS;
using System;

public class DamageDealtChange : TriggerdEffect {

	protected DamageTypes effectsDamageTypes;

	DamageDealtChange(int numberOfUses,DamageTypes damageTypesEffected,float changeDamageMultiplication, float changeDamageAddition) {
		effectType = EffectTypes.damageDealtChange;
		usesLeft = numberOfUses;
		effectsDamageTypes = damageTypesEffected;
	}

	protected float damageAdditonChange;
	protected float damageMultiplicationChange;
	public override void applyEffect(Actor applyTo) {
		DamageIncreaseData damageInfo = applyTo.getDamageIncreseContainer();
		//check to see the damage type is is one this effect works on
		if ((effectsDamageTypes & damageInfo.damageType) == damageInfo.damageType) { 
			applyTo.
		}

	}

	public override void removeEffect() {
		//nothing to do for this effect
	}
}

public struct DamageIncreaseData {
	DamageIncreaseData(float amount, DamageTypes damageType,bool directFromCard) {
		this.amount = amount;
		this.damageType = damageType;
		this.directFromCard = directFromCard;
	}

	public readonly float amount;
	public readonly DamageTypes damageType;
	public readonly bool directFromCard;
}

