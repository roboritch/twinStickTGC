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
	#region Add Card or Cards to deck

	public void addCardToDeck(System.Type cardType, float probabilityMultiplyer, bool cardRemovedOnDraw) {
		cardTypesAndProbabilitys.AddLast(new CardtypesAndProbabilities(cardType, probabilityMultiplyer, cardRemovedOnDraw));
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

	/// <summary>
	/// add multuple cards to the deck (faster than adding each indv.)
	/// </summary>
	/// <param name="cardType"></param>
	public void addCardsToDeck(System.Type[] cardType) {
		float[] probabilityMult = new float[cardType.Length];
		bool[] cardRemovedOnDraw = new bool[cardType.Length];
		for (int i = 0; i < cardType.Length; i++) {
			probabilityMult[i] = (float)cardType[i].GetField("probabiltyOfDraw").GetValue(null);
			cardRemovedOnDraw[i] = (bool)cardType[i].GetField("removeOnDraw").GetValue(null);
		}
		addCardsToDeck(cardType, probabilityMult, cardRemovedOnDraw);
	}

	public void addCardsToDeck(System.Type[] cardType, float[] probabilityMultiplyer, bool[] cardRemovedOnDraw) {
		for (int i = 0; i < cardType.Length; i++) {
			cardTypesAndProbabilitys.AddLast(new CardtypesAndProbabilities(cardType[i], probabilityMultiplyer[i], cardRemovedOnDraw[i]));
			totalProbabilityNumber += probabilityMultiplyer[i];
		}
		updateCardsInDeck(cardType.Length);
	}
	#endregion

	public int getCardsInDeck() {
		return cardsInTheDeck;
	}

	[SerializeField]
	private bool updateGUI = false; //try to update DeckGUI on method updateCardsInDeck
	private DeckGUI deckGUI;
	/// <summary>
	/// to be used by deckGui if it finds a deck
	/// </summary>
	/// <param name="gui"></param>
	public void setDeckGUI(DeckGUI gui) {
		deckGUI = gui;
		updateGUI = true;
	}
	/// <summary>
	/// update the stored count of cards in the deck by a specifyed amount
	/// </summary>
	/// <param name="cardChangeAmount"></param>
	private void updateCardsInDeck(int cardChangeAmount) {
		cardsInTheDeck += cardChangeAmount;
		//try to update the gui
		if (updateGUI) {
			if(deckGUI == null) {
				deckGUI = GetComponentInChildren<DeckGUI>();
				if(deckGUI == null) {
					Debug.LogError("No deck UI found for deck on " + name + ":" + gameObject.GetInstanceID());
				}
			} else {
				deckGUI.updateDisplayInformation();
			}
		}
	}

	// called before start
	void Awake() {
		cardTypesAndProbabilitys = new LinkedList<CardtypesAndProbabilities>();

		int iterationNumber = 2;
		int numberOfCardInstances = 3;
		System.Type[] cardsToAdd = new System.Type[iterationNumber * numberOfCardInstances];
		for (int i = 0; i < iterationNumber; i++) {
			cardsToAdd[i * numberOfCardInstances] = typeof(HitscanBase);
			cardsToAdd[i * numberOfCardInstances + 1] = typeof(ProjectileWeaponBase);
			cardsToAdd[i * numberOfCardInstances + 2] = typeof(AreaBase);
		}
		addCardsToDeck(cardsToAdd);

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