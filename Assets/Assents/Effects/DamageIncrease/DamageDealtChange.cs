using UnityEngine;
using System.Collections;
using System;

public class DamageDealtChange : Effect {

	protected DamageTypes effectsDamageTypes;

	DamageDealtChange(int numberOfUses,DamageTypes damageTypesEffected,float changeDamageMultiplication, float changeDamageAddition,bool zeroDamage) {
		canBeTriggered = true;
		effectType = EffectTypes.damageDealtChange;
		usesLeft = numberOfUses;
		effectsDamageTypes = damageTypesEffected;
	}

	protected float damageAdditonChange;
	protected float damageMultiplicationChange;
	public DamageIncreaseData damageChanges(DamageIncreaseData damageData) {
		damageData.damageAddition += damageAdditonChange;
		damageData.damageMultiplication *= damageMultiplicationChange;
		return damageData;
	}

	/// <summary>
	/// called when this is used to change the damage of somthing
	/// </summary>
	/// <returns></returns>
	public bool damageChangeEffectUsed() {
		if (--usesLeft <= 0)
			return true;
		else
			return false;
	}

	public override void applyEffect(Actor applyTo) {
		Debug.LogWarning("This effect cant be called this way! :" + GetType().Name);
	}

	public override void removeEffect() {
		//nothing to do for this effect
	}
}

public struct DamageIncreaseData {
	public DamageIncreaseData(float amount, DamageTypes damageType,bool directFromCard) {
		this.amount = amount;
		this.damageType = damageType;
		this.directFromCard = directFromCard;
		damageAddition = 0f;
		damageMultiplication = 1f;
		damage0_Flag = false;
	}

	/// <summary>
	/// will be set to 0 if there is no base amount
	/// </summary>
	public float amount;
	public readonly DamageTypes damageType;
	public readonly bool directFromCard;
	public bool damage0_Flag;
	public float damageAddition;
	public float damageMultiplication;

	public float getModifyedDamageAmount() {
		if (damage0_Flag) {
			return 0;
		}
		float finalDamage = amount;
		finalDamage += damageAddition;
		finalDamage *= damageMultiplication;
		return finalDamage;
	}
}

