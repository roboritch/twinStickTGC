using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelAnalytics : Singleton<LevelAnalytics> {
	private bool initalLevelLoad = true;

	[SerializeField]
	private string levelName = "unknown";

	private float playtime;
	private int enamiesDestroyed;
	private bool levelComplete = false;

	//completed 
	public void userFinishedLevel() {
		levelComplete = true;
	}

	public void enemyDestroyed() {
		enamiesDestroyed++;
	}

	//TODO stop if game is paused
	//don't change based on slow motion effects
	private void Update() {
		playtime += Time.unscaledDeltaTime;
	}


	private void sendLevelData() {
		Analytics.CustomEvent(levelName + "LevelEnd", new Dictionary<string, object> {
				{ "enemiesDestroyed", enamiesDestroyed},
				{ "playTime", playtime},
				{ "levelCompleted",  levelComplete}
			});
	}

	private void Awake() {
		SceneManager.sceneLoaded += levelLoaded;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="level"></param>
	private void levelLoaded(Scene sceneInfo,LoadSceneMode mode) {
		//this singleton is only active on this level
		if(!initalLevelLoad) {
			sendLevelData();
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// if app ends during level
	/// </summary>
	private void OnApplicationQuit() {
		sendLevelData();
	}
}
