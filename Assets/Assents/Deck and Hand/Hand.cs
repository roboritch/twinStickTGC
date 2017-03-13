using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// holds refrencs to the current player and all posible actions
/// sets key bidings for all card slots
/// </summary>
public class Hand : MonoBehaviour {

	public Actor curretPlayer;
	public CardSlot[] cardSlots = new CardSlot[4];
	public Deck deck;

	[SerializeField]
	private bool playerControled;
	private void initKeyBindings() {
		for (int i = 0; i < cardSlots.Length; i++) {
			KeyEvents.Instance.buttionCallbackFunctions.activateCard[i] += cardSlots[i].activateCard;
		}
	}

	void Start() {
		if(playerControled)
		initKeyBindings();
    }

}
