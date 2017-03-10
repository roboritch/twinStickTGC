using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

	#region modifyer delegates	
	public delegate float floatModifyer(float input);
	#endregion

	#region health
	[SerializeField] private float maxHealth = 100;
	[SerializeField] private float minHealth = 0; //some cards may set this > 0 for a short time
	[SerializeField] private float health;
	
	private void healthInit() {
		health = maxHealth;
	}
	#endregion

	#region action button callback
	private void initActionButtonCallback() {
		KeyEvents.Instance.buttionCallbackFunctions.mainAction = mainActionPressed;
	}

	/// <summary>
	/// called when main button is pressed (set in KeyEvents)
	/// </summary>
	public void mainActionPressed() {
		print("main button pressed");
	}
	#endregion

	#region movment attachment
	
	#endregion

	#region over time effects
	
	#endregion

	// Use this for initialization
	void Start () {
		initActionButtonCallback();
		healthInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
