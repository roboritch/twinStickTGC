using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour{
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

	/// <summary>
	/// probability this card is drawn over other cards
	/// </summary>
	public static float probabiltyOfDraw;

	/// <summary>
	/// whether or not this card is removed on draw
	/// </summary>
	public static bool removeOnDraw;

	/// <summary>
	/// description of what the card does
	/// </summary>
	public static string cardDescription;

	public abstract void displayDescription(defaultTextHolder decriptionBox);

	public abstract void useCard(Actor cardUser);

}
