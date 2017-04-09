using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : Singleton<PauseMenuHandler> {

	public bool isQuiting = false;

	[SerializeField]
	private GameObject pauseMenuPrefab;
	private GameObject pauseMenu;

	public void exit() {
		Application.Quit();
		isQuiting = true;
	}

	private void openPauseMenu() {
		if(pauseMenu == null) {
			pauseMenu = Instantiate(pauseMenuPrefab);
		}
		pauseMenu.SetActive(true);
	}

	void Start() {
		pauseMenu = GameObject.FindGameObjectWithTag("pauseMenu");
		pauseMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			openPauseMenu();
        }
	}
}
