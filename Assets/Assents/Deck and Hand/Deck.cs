using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using System.Threading;


public class Deck : MonoBehaviour {
	//use this code to check types for special card draw information from interfaces
	//typeof(IMyInterface).IsAssignableFrom(typeof(MyType)) 
	
	/// <summary>
	/// the sum of all probability multipliers in the deck
	/// this must be updated when cards are added and removed from the deck
	/// </summary>
	private float totalProbabilityNumber = 0;
	private int cardsInTheDeck = 0;
	/// <summary>
	/// this is a representation of all the cards in a players deck
	/// </summary>
	private LinkedList<CardtypesAndProbabilities> cardTypesAndProbabilities = new LinkedList<CardtypesAndProbabilities>();

	private int drawFail = 0; // prevent recursion inf loop
	/// <summary>
	/// draws a card from the deck if there is any
	/// </summary>
	/// <returns>a card from the deck, NULL if empty</returns>
	public System.Type drawCard() {
		if(cardTypesAndProbabilities.Count == 0) {
			Debug.Log("deck is empty");
			return null;
		}

		float rndNum = UnityEngine.Random.Range(0, totalProbabilityNumber); 
		float probNumber = 0;
		//calculate which card is drawn based on total probability numbers in deck
		//this of the random number as a slider and each card as a bar of variable length
		// example
		// |    #2      |     |     |   |
		// 0  rand     5     6     7  7.5        5 > 2 so card  1 is chosen
		foreach(CardtypesAndProbabilities cardsToDraw in cardTypesAndProbabilities) {
			//add the current cards probability of being drawn to the total
			probNumber += cardsToDraw.cardAttributes.probabilityOfDraw;
			if(probNumber > rndNum) {
				//check to see if card is removed from deck
				if (cardsToDraw.cardAttributes.removeOnDraw)
					removeCardFromDeck(cardsToDraw);
				return cardsToDraw.cardType;
			}
		}

		Debug.LogError("draw card fail, retrying"); // this shouldn't happen
		if (drawFail++ > 10)
			return null;
		else
			return drawCard(); 
	}

	private void removeCardFromDeck(CardtypesAndProbabilities card) {
		cardTypesAndProbabilities.Remove(card);
		totalProbabilityNumber -= card.cardAttributes.probabilityOfDraw;
		updateCardsInDeck(-1);
    }
	#region Add Card or Cards to deck

	/// <summary>
	/// probabilityMultiplyer and cardRemovedOnDraw elements are gotten from card type
	/// </summary>
	/// <param name="cardType">the class type of a card</param>
	public void addCardToDeck(System.Type cardType) {
		//look at c# reference of Type for more info on how this works
		addCardToDeck(cardType);
	}

	/// <summary>
	/// add multiple cards to the deck (faster than adding each individually)
	/// </summary>
	/// <param name="cardType"></param>
	public void addCardsToDeck(System.Type[] cardType) {
		BasicCardAttributes[] baseAttributes = new BasicCardAttributes[cardType.Length];
		
		for (int i = 0; i < cardType.Length; i++) {
			if(SaveAndLoadJson.loadStruct(SaveAndLoadJson.getResourcePath(cardType[i].Name, "CardAttr"), out baseAttributes[i])) {
				//load the error handling card
				SaveAndLoadJson.loadStruct(SaveAndLoadJson.getResourcePath("_Basic", "CardAttr"), out baseAttributes[i]);
			}
		}
		
		addCardsToDeck(cardType, baseAttributes);
	}

	/// <summary>
	/// add several cards to deck
	/// cardAttributes is used before attributes stored on disk
	/// </summary>
	/// <param name="cardType"></param>
	/// <param name="cardAttributes"></param>
	public void addCardsToDeck(System.Type[] cardType, BasicCardAttributes[] cardAttributes) {
		for (int i = 0; i < cardType.Length; i++) {
			cardTypesAndProbabilities.AddLast(new CardtypesAndProbabilities(cardType[i], cardAttributes[i]));
			totalProbabilityNumber += cardAttributes[i].probabilityOfDraw;
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
	/// update the stored count of cards in the deck by a specified amount
	/// </summary>
	/// <param name="cardChangeAmount"></param>
	private void updateCardsInDeck(int cardChangeAmount) {
		cardsInTheDeck += cardChangeAmount;
		//try to update the gui
		if (updateGUI) {
			if(deckGUI == null) {
				deckGUI = GetComponentInChildren<DeckGUI>();
				if (deckGUI == null) {
					GameObject guiObject = GameObject.Find("Deck GUI");
					if (guiObject != null)
						deckGUI = GameObject.Find("Deck GUI").GetComponent<DeckGUI>();
					if (deckGUI == null)
						Debug.LogError("No deck UI found for deck on " + name + ":" + transform.GetInstanceID());
				}
			} else {
				deckGUI.updateDisplayInformation();
			}
		}
	}
	#region Deck load and save from disk
	/// <summary>
	/// "Player Decks"
	/// </summary>
	public static readonly string playerDecks = "Player Decks";
	/// <summary>
	/// "Baddy Decks"
	/// </summary>
	public static readonly string baddyDecks = "Baddy Decks";

	private bool loadDeckFromMemory(string deckName) {
		JsonDeck deck;
		bool loadSuccess = false;
		if(playerDeck) {
			loadSuccess = SaveAndLoadJson.loadStruct(SaveAndLoadJson.getBaseFilePath(playerDecks, deckName),out deck);
		} else {
			loadSuccess = SaveAndLoadJson.loadStruct(SaveAndLoadJson.getResourcePath(baddyDecks, deckName), out deck);
		}
		if(!loadSuccess) {
			if(!SaveAndLoadJson.loadStruct(SaveAndLoadJson.getResourcePath(baddyDecks , "errorDeck"), out deck)) {
				return false;
			}
		}
		//get card class types from names
		Type[] cardClasses = new Type[deck.cardTypeName.Length];
		for(int i = 0; i < deck.cardTypeName.Length; i++) {
			cardClasses[i] = Type.GetType(deck.cardTypeName[i]);
		}
		
		addCardsToDeck(cardClasses);


		return loadSuccess;
	}



	private void initalDeckLoad(string deckName) {
		//Dev code to save a test deck
		//if (!File.Exists(SaveAndLoadXML.getBaseFilePath() + deckFolderLocation + deckName)) {
		//	saveTestDeck();
		//}
		if (loadDeckFromMemory(deckName) == false) {
			Debug.LogError("No deck found with name:" + deckName + "\n"
				+ "Deck gameObject ID: " + GetInstanceID());
			//add a card to the deck to avoid errors
			addCardToDeck(typeof(ProjectileWeaponBase));
		}
	}

	#endregion

	[SerializeField]
	private string deckName = "testDeck";
	[SerializeField]
	private bool playerDeck = true;

	// called before start
	void Awake() {
		initalDeckLoad(deckName);
	}

	// Use this for initialization
	void Start() {
		
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public struct JsonDeck {

	public JsonDeck(string[] cardNames, BasicCardAttributes[] cardBaseAttributes) {
		cardTypeName = cardNames;
	}

	public string[] cardTypeName;
}

[System.Obsolete("Json is now the method used to save and load decks")]
public struct XmlDeck {
	[XmlArrayItemAttribute("drawRemovalChecks")]
	[XmlArrayAttribute("removedOnDraw")]
	public bool[] removedOnDraw;
	[XmlArrayItemAttribute("deck")]
	[XmlArrayAttribute("card")]
	public string[] cardTypeName;
	[XmlArrayItemAttribute("drawProbMultiplyers")]
	[XmlArrayAttribute("drawProbMultiplyer")]
	public float[] probabilityMultiplyer;
}

public struct CardtypesAndProbabilities {
	
	public CardtypesAndProbabilities(System.Type cardType,BasicCardAttributes atr) {
		this.cardType = cardType;
		cardAttributes = atr;

	}

	public System.Type cardType;
	public BasicCardAttributes cardAttributes;
}

//xml implementations
#if false
	/// <summary>
	/// 
	/// </summary>
	/// <param name="deckName"></param>
	/// <returns>true if deck load success</returns>
	private bool loadDeckFromMemory(string deckName) {
		string path = SaveAndLoadXML.getBaseFilePath() + deckFolderLocation + deckName;
		XmlDeck deck;
		bool gotDeck = SaveAndLoadXML.loadXML(path,out deck);
		if (!gotDeck) {
			Debug.LogError(path);
			return false;
		}

		System.Type[] cardType = new System.Type[deck.cardTypeName.Length];
		for (int i = 0; i < cardType.Length; i++) {
			cardType[i] = System.Type.GetType(deck.cardTypeName[i], false);
			if(cardType[i] == null) {
				string errorMsg = "card not found:" + deck.cardTypeName[i] + "\n";
				errorMsg += "check if a cards name has been changed\n";
				errorMsg += "remake deck or choose another deck";
				//TODO add errors to unity main window
				Debug.LogError(errorMsg);
				return false;
			}
		}

		addCardsToDeck(cardType, deck.probabilityMultiplyer, deck.removedOnDraw);
		return true;
	}

		private void saveDeckToXml(string deckName, XmlDeck deck) {
		string path = SaveAndLoadXML.getBaseFilePath() + deckFolderLocation + deckName;
		SaveAndLoadXML.saveXML(path, deck);
	}

	
	/// <summary>
	/// save a test deck in an xml
	/// </summary>
	private void saveTestDeck() {
		int iterationNumber = 1;
		int numberOfCardInstances = 3;
		XmlDeck deckSave = new XmlDeck();
		deckSave.cardTypeName = new string[iterationNumber * numberOfCardInstances];
		deckSave.probabilityMultiplyer = new float[iterationNumber * numberOfCardInstances];
		deckSave.removedOnDraw = new bool[iterationNumber * numberOfCardInstances];

		for (int i = 0; i < iterationNumber; i++) {
			deckSave.cardTypeName[i * numberOfCardInstances] = typeof(HitscanBase).Name;
			deckSave.cardTypeName[i * numberOfCardInstances + 1] = typeof(ProjectileWeaponBase).Name;
			deckSave.cardTypeName[i * numberOfCardInstances + 2] = typeof(AreaBase).Name;

			deckSave.probabilityMultiplyer[i * numberOfCardInstances] = HitscanBase.probabiltyOfDraw;
			deckSave.probabilityMultiplyer[i * numberOfCardInstances + 1] = ProjectileWeaponBase.probabiltyOfDraw;
			deckSave.probabilityMultiplyer[i * numberOfCardInstances + 2] = AreaBase.probabiltyOfDraw;

			//setting removed on draw to false for testing
			deckSave.removedOnDraw[i * numberOfCardInstances] = false;
			deckSave.removedOnDraw[i * numberOfCardInstances + 1] = false;
			deckSave.removedOnDraw[i * numberOfCardInstances + 2] = false;
		}
		//addCardsToDeck(deckSave.cardTypeName, deckSave.probabilityMultiplyer, deckSave.removedOnDraw);
		saveDeckToXml("testDeck", deckSave);
	}

#endif