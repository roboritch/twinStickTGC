using UnityEngine;
using System.Collections;
using System;

public class DamageDealtChange : Effect {

	protected DamageTypes effectsDamageTypes;

	public DamageDealtChange() : base(false,false) {
		canBeTriggered = true;
	}

	public DamageDealtChange(int numberOfUses,DamageTypes damageTypesEffected,float damageMultiplicationChange, float damageAdditonChange, bool damageSetTo0,bool onlyWorksOnCards) : base(false, false) {
		canBeTriggered = true;
		effectType = EffectTypes.damageDealtChange;
		usesLeft = numberOfUses;
		effectsDamageTypes = damageTypesEffected;
		this.damageAdditonChange = damageAdditonChange;
		this.damageMultiplicationChange = damageMultiplicationChange;
		this.damageSetTo0 = damageSetTo0;
		this.damageTypesEffected = damageTypesEffected;
		this.onlyWorksOnCards = onlyWorksOnCards;
	}

	#region effect properties
	protected readonly int numberOfEffectProperties = 5;

	protected DamageTypes damageTypesEffected = DamageTypes.nullElm;
	protected float damageAdditonChange = 0;
	protected float damageMultiplicationChange = 0;

	protected bool onlyWorksOnCards = true;
	protected bool damageSetTo0 = true;
	#endregion


	public DamageIncreaseData damageChanges(DamageIncreaseData damageData) {
		if(damageSetTo0) {
			damageData.damage0_Flag = true;
			return damageData;
		}

		//check if the damage type is effected by this instance
		if((damageData.damageType & damageTypesEffected) != 0){
			damageData.damageAddition += damageAdditonChange;
			damageData.damageMultiplication *= damageMultiplicationChange;
		}
		return damageData;
	}

	/// <summary>
	/// called when this is used to change the damage of something
	/// </summary>
	/// <returns></returns>
	public bool damageChangeEffectUsed() {
		if (--usesLeft <= 0)
			return true;
		else
			return false;
	}

	public override void apply(Actor applyTo) {
		Debug.LogWarning("This effect cant be called this way! :" + GetType().Name);
	}

	public override void removeEffect() {
		//nothing to do for this effect
	}

	public override void setEffectProperties(EffectProperties properties) {
		damageTypesEffected = (DamageTypes)Enum.Parse(typeof(DamageTypes),properties.value[0]);
		damageAdditonChange = float.Parse(properties.value[1]);
		damageMultiplicationChange = float.Parse(properties.value[2]);
		onlyWorksOnCards = bool.Parse(properties.value[3]);
		damageSetTo0 = bool.Parse(properties.value[4]);
	}

	public override EffectProperties getEffectPropertiesStructure(bool forGUI) {
		EffectProperties properties = new EffectProperties(GetType().Name, numberOfEffectProperties, forGUI);
		if(forGUI) {
			properties.valueTypeName[0] = typeof(DamageTypes).Name;
			properties.propertyName[0] = "damageTypesEffected";

			properties.valueTypeName[1] = typeof(float).Name;
			properties.propertyName[1] = "damageAdditonChange";

			properties.valueTypeName[2] = typeof(float).Name;
			properties.propertyName[2] = "damageMultiplicationChange";

			properties.valueTypeName[3] = typeof(bool).Name;
			properties.propertyName[3] = "onlyWorksOnCards";

			properties.valueTypeName[4] = typeof(bool).Name;
			properties.propertyName[4] = "damageSetTo0";

		}

		properties.value[0] = default(DamageTypes).ToString();
		properties.value[1] = default(float).ToString();
		properties.value[2] = default(float).ToString();
		properties.value[3] = default(bool).ToString();
		properties.value[4] = default(bool).ToString();

		return properties;
	}

	public override void setCreator(Actor creator) {
		throw new NotImplementedException();
	}

	public override void initalize(Actor actor) {
		throw new NotImplementedException();
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

