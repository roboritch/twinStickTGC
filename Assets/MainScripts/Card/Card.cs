using System;
using UnityEngine;

public abstract class Card {
	//this section of code should be placed at the start of all new cards 
	//to initialize the cards values
	#region card children initialization code
#if false
	#region Initialization of static members
	static CardName() { } //insures these values are overwritten properly
	#endregion

	#region initialization of parent vars
	// sprite is done via the unity inspector by 
	// clicking on this script in the project assets window

	public _CardName_() : base() {
		
	}
	#endregion
#endif
	#endregion

	public Card() {
		if(CardAttributeLoader.LoadBasicCardAttributesFromJsonInResorceFolder(GetType().Name, out basicAttrabutes)) {
			
		}
		Debug.Log(basicAttrabutes.ToString());
	}

	/// <summary>
	/// this returns the class type name (of the current highest class child) with "/" at the end
	/// </summary>
	/// <returns></returns>
	protected string getCardResorceFolderPath() {
		return GetType().Name + "/";
	}

	//these variables must be initialized before a cards constructor is done 
	#region resources used by deck creator

	public BasicCardAttributes basicAttrabutes;

	#endregion


	/// <summary>
	/// tells the card that the user wants to use it
	/// will return true if the card is ready to be removed from the hand
	/// </summary>
	/// <param name="cardUser"></param>
	/// <returns>true if card used,false if card not used</returns>
	public abstract bool useCard(Actor cardUser);

	/// <summary>
	/// All on disk assents required for the card are loaded into memory
	/// </summary>
	public virtual void cacheResorces() {
		PrefabResorceLoader.Instance.loadSprite(basicAttrabutes.cardIconPath);
		PrefabResorceLoader.Instance.loadSprite(basicAttrabutes.cardArtPath);
		PrefabResorceLoader.Instance.loadTextAsset(basicAttrabutes.cardDescriptionPath);
	}

	/// <summary>
	/// called when something requires the card to be removed from the CardSlot
	/// </summary>
	public abstract void destroyCard();
}

public static class CardAttributeLoader{
	//TODO change this to use assent bundles at some point
	/// <summary>
	/// load the text Assent CardAttr
	/// </summary>
	/// <param name="cardName"></param>
	/// <param name="cardAttr"></param>
	/// <returns></returns>
	public static bool LoadBasicCardAttributesFromJsonInResorceFolder(string cardName, out BasicCardAttributes cardAttr) {
		cardAttr = default(BasicCardAttributes);
		TextAsset cardAttrJson = PrefabResorceLoader.Instance.loadTextAsset(cardName + "/CardAttr"); //Note: resorce.load does not need file extension
		bool loaded = false;
		if(cardAttrJson != null) {
			loaded = SaveAndLoadJson.loadStructFromString(out cardAttr, cardAttrJson.text);
		}
		if(!loaded) {
			if(Application.isEditor) {
				//TODO create CardPramSetter
				Debug.Log("Card attributes not set" + cardName +"\n" 
					+ "create using editor using CardPramSetter");
			} else {
				Debug.LogError("something has gone wrong, there are no card Attributes in " + cardName);
			}
		}
		return loaded; 
	}
}

//WARING this struct should not contain references of any kind (including arrays) 
//WARNING MAJOR WARN If this is changed all cards json files must be updated
[System.Serializable]
public struct BasicCardAttributes {
	/// <summary>
	/// helper to load vars from json more easily 
	/// </summary>
	/// <param name="vars"></param>
	public void loadVars(BasicCardAttributes vars) {
		this = vars;
	}

	//these should be handled by
	#region Resource Path Names
	/// <summary>
	/// a smallish icon that is easily readable and represents the card
	/// Resources(not included)/_CardName_/icon
	/// the standard right now (month-day 05/06) is to have all icons named "icon"
	/// </summary>
	public string cardIconPath;

	/// <summary>
	/// Art that is displayed on a card
	/// the standard right now (month-day 05/06) is to have all art named "card art"
	/// </summary>
	public string cardArtPath;

	/// <summary>
	/// Path to the card description, having this as a path saves memory at runtime if 
	/// the description is not needed
	/// the standard right now (month-day 05/06) is to have all Descriptions named "description"
	/// </summary>
	public string cardDescriptionPath;
	#endregion

	/// <summary>
	/// The name displayed in the view window (can be different from the class name)
	/// </summary>
	public string cardName;

	/// <summary>
	/// time till a new card is drawn when this one is used
	/// </summary>
	public float cardReloadTime_seconds;

	/// <summary>
	/// amount of resources this card uses
	/// </summary>
	public float cardResorceCost;

	/// <summary>
	/// all of the elements that the card can use
	/// </summary>
	public DamageTypes cardElements;

	/// <summary>
	/// this specifies basic information about how a card is used
	/// and where it is stored on a character
	/// </summary>
	public CardLocation cardType;

	/// <summary>
	/// probability this card is drawn over other cards
	/// </summary>
	public float probabilityOfDraw;

	/// <summary>
	/// whether or not this card is removed on draw
	/// mostly used for debugging but could be used for some cards
	/// </summary>
	public bool removeOnDraw;

	/// <summary>
	/// describes how many of this card is allowed  in a deck
	/// </summary>
	public int numberOfCardAllowedInDeck;
}

//linked with actor equipment
public enum CardLocation {
	///card is never really exists in the world
	///this includes cards that create projectiles on use
	instant = 1 << 0,
	///creates some object in the world
	persistant = 1 << 1,

	//slots
	above = 1 << 2,
	right = 1 << 3,
	below = 1 << 4,
	left = 1 << 5,
	onTopOf = 1 << 6,
	///if this slot is used no other slot should be used
	noSlot = 1 << 7,

	//the card takes up all slots
	all = 1 << 8,
}


#if false
// char sheet used as reference https://s-media-cache-ak0.pinimg.com/originals/02/2d/45/022d4533e8fb9351a42f5d904cd69da7.png
/// <summary>
/// only a few of these will be used in the initial releases 
/// Have a dictionary with these as keys and equipment created by cards as values
/// on an actor
/// </summary>
//menu values are ordered from the top of the body to the bottom
[Flags]
public enum CardTypes {
	///card is never really exists in the world
	///this includes cards that create projectiles on use
	instant = 1 << 0,
	///creates some object in the world
	persistant = 1 << 1,

	head = 1 << 2,
	eyes = 1 << 3,
	neck = 1 << 4,

	sholderR = 1 << 5,
	sholderL = 1 << 6,

	upperBack = 1 << 7,
	lowerBack = 1 << 8,
	chest = 1 << 9,
	lowerBody = 1 << 10,

	upperArmR = 1 << 11,
	upperArmL = 1 << 12,
	lowerArmR = 1 << 13,
	lowerArmL = 1 << 14,

	handR = 1 << 15,
	handL = 1 << 16,
	///as a special case this has 10 slots on an actor
	ring = 1 << 17,
	belt = 1 << 18,

	legTopR = 1 << 19,
	legTopL = 1 << 20,

	legBottemR = 1 << 21,
	legBottemL = 1 << 22,

	footR = 1 << 23,
	footL = 1 << 24,
	///the card uses all flags that are set
	allSet = 1 << 25,
	///the card can fit in any slot specified
	onlyOne = 1 << 26,
}
#endif