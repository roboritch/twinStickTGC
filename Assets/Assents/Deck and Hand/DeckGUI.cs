using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeckGUI : MonoBehaviour {
	[SerializeField]
	private Deck deck;
	private defaultTextHolder cardCountText;

	public void updateDisplayInformation() {
		if(deck == null) {
			Debug.LogWarning("no deck found, can't display information");
			return;
		}
		cardCountText.newText(deck.getCardsInDeck().ToString());
	}

	// Use this for initialization
	void Start() {
		if(deck == null) { // set if not specifyed in inspector
			deck = GetComponent<Deck>();
		}
		cardCountText = GetComponentInChildren<defaultTextHolder>();
	}
}
