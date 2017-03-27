using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaControls : MonoBehaviour {


	public DamageArea[] damageAreas;

	// Use this for initialization
	void Awake () {
		damageAreas = GetComponentsInChildren<DamageArea>(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
