using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameWorld : MonoBehaviour {

	private bool initalizing = true;
	private float currentTimeScale = 1;

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
		if (initalizing) {
			initalizing = false;
			currentTimeScale = Time.timeScale;
			return;
		}
		Pause();
	}

	void OnDisable() {
		unPause();
	}
	
	void Update () {
		
	}
}
