using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	#region health
	[SerializeField] private float maxHealth = 100;
	[SerializeField] private float minHealth = 0; //some cards may set this > 0 for a short time
	[SerializeField] private float health;
	
	private void healthInit() {
		health = maxHealth;
	}
	#endregion

	#region movment atachment
	#endregion

	#region over time efects
		
	#endregion

	// Use this for initialization
	void Start () {
		healthInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
