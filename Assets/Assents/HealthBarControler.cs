using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControler : MonoBehaviour {

	[SerializeField]
	private RectTransform healthAmount;
	[SerializeField]
	private defaultTextHolder healthNumber;
	private void initHealthDisplay() {
		healthBarMaxSize = healthAmount.sizeDelta.x;
	}

	private float healthBarMaxSize = 100f;
	public void updateHealthDispley(float maxHealth,float currentHealth) {
		float healthBarSize = currentHealth / maxHealth;
		healthBarSize *= healthBarMaxSize; //get % of max size relative to player health
		healthAmount.sizeDelta = new Vector2(healthBarSize,healthAmount.sizeDelta.y);
		healthNumber.newText(currentHealth + "/" + maxHealth);
	}

	void Awake() {
		initHealthDisplay();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
