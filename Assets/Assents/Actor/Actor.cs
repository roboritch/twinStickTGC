using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		initButtonCallbacks();
		healthInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
