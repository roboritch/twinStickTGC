using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour,IDamageable {
	public bool blocksDamage(float amount, DamageTypes damageType) {
		return true;
	}

	public bool ignoreDamage(DamageSources damageSorce, DamageTypes damageType) {
		return false;
	}

	public bool takeDamage(float amount, DamageTypes damageType, DamageSources damageSorce) {
		return false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
