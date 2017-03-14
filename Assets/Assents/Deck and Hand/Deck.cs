using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
	//use this code to check types for special card draw information from interfaces
	//typeof(IMyInterface).IsAssignableFrom(typeof(MyType)) 

	private float totalProbabilityNumber = 0;
	private int cardsInTheDeck = 0;
	private LinkedList<CardtypesAndProbabilities> cardTypesAndProbabilitys;

	private int drawFail = 0; // prevent recurshon inf loop
	/// <summary>
	/// 
	/// </summary>
	/// <returns>a card from the deck, NULL if empty</returns>
	public System.Type drawCard() {
		if(cardTypesAndProbabilitys.Count == 0) {
			Debug.Log("deck is empty");
			return null;
		}

		float rndNum = Random.Range(0, totalProbabilityNumber);
		float probNumber = 0;
		foreach (var item in cardTypesAndProbabilitys) {
			probNumber += item.probabilityMultiplyer;
			if(probNumber > rndNum) {
				if (item.removedOnDraw)
					removeCardFromDeck(item);
				return item.cardType;
			}
		}

		Debug.LogError("draw card fail, retrying"); // this shouldn't happen
		if (drawFail++ > 10)
			return null;
		else
			return drawCard(); 
	}

	private void removeCardFromDeck(CardtypesAndProbabilities card) {
		cardTypesAndProbabilitys.Remove(card);
		updateCardsInDeck(-1);
    }

	public void addCardToDeck(System.Type cardType, float probabilityMultiplyer,bool cardRemovedOnDraw) {
		cardTypesAndProbabilitys.AddLast(new CardtypesAndProbabilities(cardType, probabilityMultiplyer,cardRemovedOnDraw));
		totalProbabilityNumber += probabilityMultiplyer;
		updateCardsInDeck(1);
    }

	private defaultTextHolder cardCount;
	private void updateCardsInDeck(int cardChangeAmount) {
		cardsInTheDeck += cardChangeAmount;
		cardCount.setNewDefautText(cardsInTheDeck.ToString());
	}

	// called before start
	void Awake() {
		cardCount = GetComponentInChildren<defaultTextHolder>();
		cardTypesAndProbabilitys = new LinkedList<CardtypesAndProbabilities>();
		addCardToDeck(typeof(ProjectileGun), ProjectileGun.probabiltyOfDraw, ProjectileGun.removeOnDraw);
	}

	// Use this for initialization
	void Start() {
		
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public struct CardtypesAndProbabilities {
	
	public CardtypesAndProbabilities(System.Type cardType, float probabilityMultiplyer,bool removedOnDraw) {
		this.cardType = cardType;
		this.probabilityMultiplyer = probabilityMultiplyer;
		this.removedOnDraw = removedOnDraw;
    }

	public bool removedOnDraw;
	public System.Type cardType;
	public float probabilityMultiplyer;
}