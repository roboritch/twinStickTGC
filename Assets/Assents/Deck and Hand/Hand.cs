using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO seperate player and AI hands and crate a base class
//TODO seperate visual and logic elements from these components
/// <summary>
/// holds refrencs to the current player and all posible actions
/// sets key bidings for all card slots
/// </summary>
public class Hand : MonoBehaviour {

	public Actor handUser;
	public CardSlot[] cardSlots = new CardSlot[4];
	public Deck deck;

	[SerializeField]
	private bool playerControled;
	private void initKeyBindings() {
		for (int i = 0; i < cardSlots.Length; i++) {
			KeyEvents.Instance.buttionCallbackFunctions.activateCard[i] += cardSlots[i].activateCard;
		}
	}

	/// <summary>
	/// used to prevent the player activating cards when dead
	/// </summary>
	public void removeKeyBindings() {
		if(!quiting) //prevents null errors on quit
		for (int i = 0; i < cardSlots.Length; i++) {
			KeyEvents.Instance.buttionCallbackFunctions.activateCard[i] = KeyEvents.Instance.emptyCallback;
		}
	}

	/// <summary>
	/// Debug code for now
	/// </summary>
	private void drawInitalCardFromDeck() {
		if(deck != null)
		for (int i = 0; i < 3; i++) {
			System.Type card = deck.drawCard();
			if(card != null)
				cardSlots[i].receiveCard((Card)Activator.CreateInstance(card));
		}

	}

	public void clearHand() {
		for (int i = 0; i < cardSlots.Length; i++) {
			cardSlots[i].removeCard();
		}
	}

	//if player is not set in inspector try to find it
	void Awake() {
		if(handUser == null) {
			handUser = GetComponent<Actor>();
			if (handUser == null) {
				GameObject player = GameObject.FindGameObjectWithTag("Player");
				if(player != null) {
					handUser = player.GetComponent<Actor>();
					if(deck == null) {
						deck = player.GetComponent<Deck>();
					}
				}
			}
		}
	}

	void tryGetHandUser() {
		
	}

	void Start() {
		if(playerControled)
			initKeyBindings();
		drawInitalCardFromDeck();
    }

	private bool quiting = false;
	void OnApplicationQuit() {
		quiting = true;
	}

}
