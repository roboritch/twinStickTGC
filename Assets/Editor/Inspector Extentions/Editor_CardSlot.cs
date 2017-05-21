using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardSlot))]
public class Editor_CardSlot : Editor {

	private MonoScript selectedCard;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		CardSlot myTarget = (CardSlot)target;

		//TODO improve card selection
		if(Application.isPlaying) {
			selectedCard = (MonoScript)EditorGUILayout.ObjectField("Card", selectedCard, typeof(MonoScript), false);
			if(selectedCard != null)
				if(selectedCard.GetClass().IsSubclassOf(typeof(Card))) {
					if(GUILayout.Button("make card")) {
						myTarget.removeCard();
						myTarget.receiveCard((Card)System.Activator.CreateInstance(selectedCard.GetType()));
					}

				}

		}
	}
}