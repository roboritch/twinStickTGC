using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckList : MonoBehaviour {

	[SerializeField]
	private GameObject deckListPrefab;

	[SerializeField]
	private Transform cardListDisplay;

	//TODO implement IComparer on CardDisplayController
	/// <summary>
	/// list of current cards in deck sorted by cost (lowest at top)
	/// </summary>
	private List<CardDisplayController> listOfCards = new List<CardDisplayController>();
	//TODO add card modifiers that can be placed over cards to slightly modify basic values

	//TODO add handler for displaying 2 or more of the same card
	//TODO check max amount of card in deck before adding
	public void addCardToDeck(Card card) {
		CardDisplayController dControler = Instantiate(deckListPrefab, cardListDisplay).GetComponent<CardDisplayController>();
		listOfCards.Add(dControler);
		dControler.setCardDisplay(card, this);
		orderList_cost();
	}

	public void removeCardFromDeck(CardDisplayController cardDisplay) {

	}

	private void orderList_cost() {
		int locationInList = 0;
		listOfCards.Sort(new CardDisplayControllerSort_cost());
		foreach(CardDisplayController display in listOfCards) {
			display.transform.SetSiblingIndex(locationInList++);
		}
		LayoutRebuilder.MarkLayoutForRebuild(cardListDisplay as RectTransform);
	}



	private void Start() {
		for(int i = 0; i < cardListDisplay.childCount; i++) {
			UnityExtentionMethods.destoryAllChildren(cardListDisplay.GetChild(i));
		}
	}



}
