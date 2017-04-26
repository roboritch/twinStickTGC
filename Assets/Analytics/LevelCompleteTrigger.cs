using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour, ITriggerable {
	public void trigger() {
		if(LevelAnalytics.Exists() && !externalApplicationEnd)
			LevelAnalytics.Instance.userFinishedLevel();
	}

	private bool externalApplicationEnd = false;

	public void OnApplicationQuit() {
		externalApplicationEnd = true;
	}
}
