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

	/// <summary>
	/// the pause menu may call this on startup
	/// </summary>
	/// <param name="pm"></param>
	public void setPauseMenu(GameObject pm) {
		pauseMenu = pm;
	}

	public bool pauseMenuExists() {
		return pauseMenu != null;
	}

	/// <summary>
	/// Open the pause menu or create i
	/// </summary>
	private void openPauseMenu() {
		if(pauseMenu == null) {
			if(pauseMenuPrefab != null) {
				pauseMenu = Instantiate(pauseMenuPrefab);
			} else {
				Debug.LogError("no pause menu found in " + transform.GetInstanceID());
			}
		}
		pauseMenu.SetActive(true);
	}

	void Start() {
		pauseMenu = GameObject.FindGameObjectWithTag("pauseMenu");
		if(pauseMenu != null)
			pauseMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//HACK only let pause menu show up if an actor exists
		if (Input.GetKeyUp(KeyCode.Escape) && FindObjectsOfType<Actor>() != null) {
			openPauseMenu();
        }
	}
}
