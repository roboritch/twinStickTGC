using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel1 : MonoBehaviour {

	public string levelName;
	public void LoadLevel() {
		SceneManager.LoadScene(levelName);
    }

}
