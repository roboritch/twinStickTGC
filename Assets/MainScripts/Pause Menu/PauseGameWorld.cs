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

	private void Awake() {
		//WARNING this assumes PauseGameWorld transform is parented to a canvas
		if(PauseMenuHandler.Instance.pauseMenuExists()) {
			Debug.LogWarning("Pause menu already exists! \n destroying this one");
			UnityExtentionMethods.destoryAllChildren(transform.parent);
			return;
		}
		PauseMenuHandler.Instance.setPauseMenu(transform.parent.gameObject);
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
