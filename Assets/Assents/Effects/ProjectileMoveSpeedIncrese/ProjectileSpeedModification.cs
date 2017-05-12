using UnityEngine;
using System.Collections;
using System;

public class ProjectileSpeedModification : TimedEffect {
	public override void apply(Actor applyTo) {
		Debug.LogWarning("This effect cant be called this way! :" + GetType().Name);
	}
	#region base effectProperties
	/// <summary>
	/// increase of 50% = an increase multiplier of 0.5f
	/// </summary>
	protected float increseMultiplyer = 0f;
	#endregion

	public ProjectileSpeedModification(bool requiresCreator, bool mustBeInitalized) : base(requiresCreator, mustBeInitalized) {
	}

	public virtual float getProjectileSpeedIncreseMultiplyer(float baseSpeed) {
		return increseMultiplyer;
	}

	public override bool incrmentTimer(float time_seconds) {
		timeLeft_seconds -= timeLeft_seconds;
		if(timeLeft_seconds <= 0) {
			usesLeft = 0;
			return true;
		}
		return false;
	}

	public override void removeEffect() {
		//nothing here
	}

	public override void setCreator(Actor creator) {
		throw new NotImplementedException();
	}

	public override void initalize(Actor actor) {
		throw new NotImplementedException();
	}

	public override void setEffectProperties(EffectProperties properties) {
		increseMultiplyer = float.Parse(properties.value[0]);
	}

	public override EffectProperties getEffectPropertiesStructure(bool forGUI) {
		EffectProperties properties = new EffectProperties(GetType().Name, 1, forGUI);
		if(forGUI) {
			properties.valueTypeName[0] = typeof(float).Name;
			properties.propertyName[0] = "increseMultiplyer";

		}
		properties.value[0] = default(float).ToString();

		return properties;
	}
}
