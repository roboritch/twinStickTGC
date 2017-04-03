using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
	//use this code to check types for special card draw information from interfaces
	//typeof(IMyInterface).IsAssignableFrom(typeof(MyType)) 
	
	/// <summary>
	/// the sum of all probability multiplyers in the deck
	/// this must be updated when cards are added and removed from the deck
	/// </summary>
	private float totalProbabilityNumber = 0;
	private int cardsInTheDeck = 0;
	/// <summary>
	/// this is a representation of all the cards in a players deck
	/// </summary>
	private LinkedList<CardtypesAndProbabilities> cardTypesAndProbabilitys;

	private int drawFail = 0; // prevent recurshon inf loop
	/// <summary>
	/// draws a card from the deck if there is any
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
		totalProbabilityNumber -= card.probabilityMultiplyer;
		updateCardsInDeck(-1);
    }

	public void addCardToDeck(System.Type cardType, float probabilityMultiplyer,bool cardRemovedOnDraw) {
		cardTypesAndProbabilitys.AddLast(new CardtypesAndProbabilities(cardType, probabilityMultiplyer,cardRemovedOnDraw));
		totalProbabilityNumber += probabilityMultiplyer;
		updateCardsInDeck(1);
    }

	/// <summary>
	/// probabilityMultiplyer and cardRemovedOnDraw elements are gotten from card type
	/// </summary>
	/// <param name="cardType">the class type of a card</param>
	public void addCardToDeck(System.Type cardType) {
		//look at c# refrence of Type for more info on how this works
		addCardToDeck(cardType, (float)cardType.GetField("probabiltyOfDraw").GetValue(null), (bool)cardType.GetField("removeOnDraw").GetValue(null));
	}

	private defaultTextHolder cardCount;
	private void updateCardsInDeck(int cardChangeAmount) {
		cardsInTheDeck += cardChangeAmount;
		cardCount.addNewTextToDefalt(cardsInTheDeck.ToString());
	}

	// called before start
	void Awake() {
		cardCount = GetComponentInChildren<defaultTextHolder>();
		cardTypesAndProbabilitys = new LinkedList<CardtypesAndProbabilities>();

		for (int i = 0; i < 2; i++) {
			//addCardToDeck(typeof(HitscanBase));
			//addCardToDeck(typeof(ProjectileWeaponBase));
			addCardToDeck(typeof(AreaBase));
		}
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