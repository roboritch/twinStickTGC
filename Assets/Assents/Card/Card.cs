using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {
	//this section of code should be placed at the start of all new cards 
	//to initialize the cards values
	#region card children initialization code
#if false
	#region Initialization of static members
	static CardName() { } //insures these values are overwritten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initialization of parent vars
	// sprite is done via the unity inspector by 
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
	/// this returns the class type name (of the current highest class child)
	/// </summary>
	/// <returns></returns>
	protected string getIconPath() {
		return GetType().Name + "/";
	}

	/// <summary>
	/// image that appears in the hand
	/// </summary>
	public Sprite cardArt;

	/// <summary>
	/// time till a new card is drawn when this one is used
	/// </summary>
	public float cardReloadTime_seconds;

	/// <summary>
	/// amount of resources this card uses
	/// </summary>
	public float cardResorceCost;

	//These variables must be available before an instance of this class is created
	#region Required Initial Vars
	static Card() {} //enforces order of static variable initialization in children
	/// <summary>
	/// probability this card is drawn over other cards
	/// WARNING reflection (finding var by string name) is used on this var, DO NOT change its name
	/// </summary>
	public static readonly float probabiltyOfDraw;

	/// <summary>
	/// whether or not this card is removed on draw
	/// WARNING reflection (finding var by name) is used on this var, DO NOT change its name
	/// </summary>
	public static readonly bool removeOnDraw;


	//this var is not finalized, it should probably be stored in a text file
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
