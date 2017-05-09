using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;

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
	private LinkedList<CardtypesAndProbabilities> cardTypesAndProbabilitys = new LinkedList<CardtypesAndProbabilities>();

	private int drawFail = 0; // prevent recursion inf loop
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
		//look at c# reference of Type for more info on how this works
		addCardToDeck(cardType);
	}

	/// <summary>
	/// add multiple cards to the deck (faster than adding each individually)
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
	private readonly string deckFolderLocation = "DeckSaves/";
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
	
	public CardtypesAndProbabilities(System.Type cardType, float probabilityMultiplyer,bool removedOnDraw) {
		this.cardType = cardType;
		this.probabilityMultiplyer = probabilityMultiplyer;
		this.removedOnDraw = removedOnDraw;
    }
	[XmlElement("removedOnDraw")]
	public bool removedOnDraw;
	[XmlElement("card")]
	public System.Type cardType;
	[XmlElement("probability of draw multiplyer")]
	public float probabilityMultiplyer;
}