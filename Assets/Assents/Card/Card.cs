using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {
	
	
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
	/// 
	/// </summary>
	/// <param name="cardUser"></param>
	/// <returns>true if card used,false if card not used</returns>
	public abstract bool useCard(Actor cardUser);

}
