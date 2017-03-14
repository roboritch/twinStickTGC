using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour , IDamageable {

	#region modifyer delegates	
	public delegate float floatModifyer(float input);
	#endregion

	#region health
	[SerializeField] private float health_MAX = 100;
	[SerializeField] private float health_MIN = 0; //some cards may set this > 0 for a short time
	[SerializeField] private float health;

	private void healthInit() {
		health = health_MAX;
	}

	public void takeDamage(float amount) {
		health -= amount;
	}

	public new Collider2D collider;
	#endregion

	//all conections to KeyEvents shoud be here and be disablable
	#region button callbacks
	[SerializeField] private bool playerControled = true;
	private void initButtonCallbacks() {
		if (playerControled) { 
			KeyEvents.Instance.buttionCallbackFunctions.mainAction += mainActionPressed;
		}
	}

	/// <summary>
	/// called when main button is pressed (set in KeyEvents)
	/// </summary>
	public void mainActionPressed() {
		
	}
	#endregion

	#region over time effects

	#endregion

	#region Hand of Cards
	
	#endregion

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider2D>();
		initButtonCallbacks();
		healthInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region card helper functions
	public Vector2 get2dPostion() {
		return new Vector2(transform.position.x, transform.position.y);
    }
	#endregion

}
