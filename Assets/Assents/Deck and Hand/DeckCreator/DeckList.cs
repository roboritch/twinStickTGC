using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class DeckList : MonoBehaviour {

	[SerializeField]
	private InputField deckName;

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
		listOfCards.Remove(cardDisplay);
		UnityExtentionMethods.destoryAllChildren(cardDisplay.transform);
	}

	public void clearDeck() {
		foreach(CardDisplayController cardDisplayer in listOfCards) {
			UnityExtentionMethods.destoryAllChildren(cardDisplayer.transform);
		}
	}

	public void saveDeck() {
		string[] cardClassNames = new string[listOfCards.Count];
		BasicCardAttributes[] cardsAttributes= new BasicCardAttributes[listOfCards.Count];
		Card c;
		int index = 0;
		foreach(CardDisplayController item in listOfCards) { 
			c = item.getCardRepresented();
			cardClassNames[index] = c.GetType().Name;
			cardsAttributes[index++] = c.basicAttrabutes;
		}
		
		JsonDeck deckSave = new JsonDeck(cardClassNames, cardsAttributes);
		SaveAndLoadJson.saveStruct(SaveAndLoadJson.getResourcePath(Deck.playerDecks + deckName.text), deckSave);
	}

	public void loadDeck(string deckName) {
		clearDeck();

		//TODO add confirmation screen when loading/exiting deck editor

		JsonDeck deckLoad;
		if(SaveAndLoadJson.loadStruct(SaveAndLoadJson.getResourcePath(Deck.playerDecks + deckName), out deckLoad) == false) {
			Debug.LogError("selected deck does not exist");
			return;
		}

		Card c = null;
		object obj;
		for(int i = 0; i < deckLoad.cardTypeName.Length; i++) {
			obj = Activator.CreateInstance(Type.GetType(deckLoad.cardTypeName[i]));
			if(obj != null) {
				if(obj is Card) {
					c = (Card)obj;
					c.basicAttrabutes = deckLoad.cardBaseAttributes[i];
					addCardToDeck(c);
					continue;
				}
			}
			Debug.LogError("card " + deckLoad.cardTypeName[i]+ " could not be loaded\n" +
				"check that the card class with that name exists");
		}
		

		
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
