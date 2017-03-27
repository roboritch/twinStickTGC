using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {
	//this section of code should be placed at the start of all new cards 
	//to initalize the cards values
	#region card children initalization code
#if false
	#region Initalization of static members
	static CardName() { } //insures these values are overwriten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initalization of parent vars
	// sprite is done via the unity inspecter by 
	// clicking on this script in the project assets window

	protected string getIconPath() {
		return GetType().Name + "/";
	}

	public CardName() {
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
	}
	#endregion
#endif
	#endregion


	/// <summary>
	/// image that apears in the hand
	/// </summary>
	public Sprite cardArt;

	/// <summary>
	/// time till a new card is drawn when this one is used
	/// </summary>
	public float cardReloadTime_seconds;

	/// <summary>
	/// amount of resorces this card uses
	/// </summary>
	public float cardResorceCost;

	//These variables must be avalible before an instance of this class is created
	#region Required Inital Vars
	static Card() {} //efforces order of static variable initalization 
	/// <summary>
	/// probability this card is drawn over other cards
	/// </summary>
	public static readonly float probabiltyOfDraw;

	/// <summary>
	/// whether or not this card is removed on draw
	/// </summary>
	public static readonly bool removeOnDraw;


	//this var is not finalized, it should probobly be stored in a text file
	/// <summary>
	/// description of what the card does
	/// </summary>
	public static string cardDescription;
#endregion

	public abstract void displayDescription(defaultTextHolder decriptionBox);

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
	public abstract void cacheResorces();

	/// <summary>
	/// called when something requires the card to be removed from the CardSlot
	/// </summary>
	public abstract void destroyCard();
}
