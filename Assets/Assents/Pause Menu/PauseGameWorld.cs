using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameWorld : MonoBehaviour {

	private float currentTimeScale;

	private void Pause() {
		currentTimeScale = Time.timeScale;
		Time.timeScale = 0; // this should stop all update methods
	}

	private void unPause() {
		Time.timeScale = currentTimeScale;
	}

	public void exitPauseMenu() {
		unPause();
		transform.parent.gameObject.SetActive(false);
	}

	void OnEnable () {
		Pause();
    }

	//initalization (On Enable is called first)
	void Start() {
		unPause();
	}

	void OnDisable() {
		unPause();
	}
	
	void Update () {
		
	}
}
