using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderCamera : MonoBehaviour {

	[SerializeField]
	private bool disableOnCreation;

	// Use this for initialization
	void Awake () {
		GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		if (disableOnCreation) {
			gameObject.SetActive(false);
		}
		GetComponent<Canvas>().enabled = true;
    }
	
}
