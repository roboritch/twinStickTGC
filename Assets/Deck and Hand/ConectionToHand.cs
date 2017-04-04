using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// notifyes the hand of any important effects on the player (not to be used for npc or remote players)
/// </summary>
public class ConectionToHand : MonoBehaviour {

	void Start() {
		GameObject obj = GameObject.FindGameObjectWithTag("PlayerHand");
		if(obj != null)
			hand = obj.GetComponent<Hand>();
	}

	private Hand hand;
	public Hand getHand() {
		return hand;
	}
	void OnDestroy() {
		if(hand != null) {
			hand.removeKeyBindings();
		}
	}
	
}
