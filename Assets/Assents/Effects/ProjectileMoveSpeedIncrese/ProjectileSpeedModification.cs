using UnityEngine;
using System.Collections;
using System;

public class ProjectileSpeedModification : TimedEffect {
	public override void applyEffect(Actor applyTo) {
		Debug.LogWarning("This effect cant be called this way! :" + GetType().Name);
	}

	/// <summary>
	/// increse of 50% = an increse multipyer of 0.5f
	/// </summary>
	protected float increseMultiplyer = 0f;
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
}
