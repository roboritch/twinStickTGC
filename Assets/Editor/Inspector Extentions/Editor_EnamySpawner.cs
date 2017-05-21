using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnamySpawner))]
public class Editor_EnamySpawner : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EnamySpawner myTarget = (EnamySpawner)target;

		if(GUILayout.Button("spawnEnamy")) {
			myTarget.spawnEnamy();
		}

	}

}
