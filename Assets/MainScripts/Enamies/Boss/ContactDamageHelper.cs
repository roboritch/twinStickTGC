using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageHelper : MonoBehaviour {
	[SerializeField]
	private Actor controler;

	[SerializeField]
	private ContactDamageComponent comp;

	// Use this for initialization
	void Start () {
		comp.initDamage(controler, 10f, DamageTypes.physical_normal, 5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
