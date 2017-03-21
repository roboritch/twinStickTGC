using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileBase : MonoBehaviour {
	Collider2D collider;
	
	void Update() {
		hitTrigger();
	}

	void Start() {
		collider = GetComponent<Collider2D>();	
	}

	protected void hitTrigger() {

	}



}
