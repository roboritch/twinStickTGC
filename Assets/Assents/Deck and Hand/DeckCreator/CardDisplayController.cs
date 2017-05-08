using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayController : MonoBehaviour {


	/*TODO all these objects should have controller 
	 * scripts so tool tips and other things can be displayed */
	#region Card Display Vars
	[SerializeField]
	private Image elementalIcon;
	[SerializeField]
	private Image cardIcon;
	[SerializeField]
	private Image cardArt;
	/// <summary>
	/// this image helps describe how the card is used
	/// </summary>
	[SerializeField]
	private Image cardType;
	[SerializeField]
	private Text cardCost;
	[SerializeField]
	private Text cardProbability;
	[SerializeField]
	private Text cardName;
	[SerializeField]
	private Text description;
	[SerializeField]
	private Text cardReloadTime;
	[SerializeField]
	private Text cardsAlowedInDeck;

	#endregion

	private Card cardRepresented;
	public void setCardDisplay(Card card) {
		// TODO create elemental icons
		//ElementalIcon.sprite = getElamentalIcon
		cardRepresented = card;
		displayCardElements();
		displayCardIcon();
		displayCardArt();
		displayCardType();
		displayCardCost();
		displayCardDrawProbability();
		displayCardName();
		displayCardDescription();
		displayCardsAlowedInDeck();

	}

	private void displayCardElements() {

	}

	private void displayCardIcon() {
		Sprite iconVal = CardPrefabResorceLoader.Instance.loadSprite(cardRepresented.basicAttrabutes.cardIconPath);
		if(iconVal == null) {
			iconVal = CardPrefabResorceLoader.Instance.loadSprite("_Basic/icon");
		}
		cardIcon.sprite = iconVal;
	}

	private void displayCardArt() {
		Sprite art = CardPrefabResorceLoader.Instance.loadSprite(cardRepresented.basicAttrabutes.cardArtPath);
		if(art == null) {
			art = CardPrefabResorceLoader.Instance.loadSprite("_Basic/card art");
		}
		cardArt.sprite = art;
	}

	private void displayCardType() {

	}

	private void displayCardCost() {
		cardCost.text = cardRepresented.basicAttrabutes.cardResorceCost.ToString();
	}
	 
	private void displayCardDrawProbability() {
		cardProbability.text = "x" + cardRepresented.basicAttrabutes.probabilityOfDraw.ToString();
	}

	private void displayCardName() {
		cardName.text = cardRepresented.basicAttrabutes.cardName;
	}

	private void displayCardDescription() {
		TextAsset text = CardPrefabResorceLoader.Instance.loadTextAsset(cardRepresented.basicAttrabutes.cardDescriptionPath);
		if(text == null) {
			text = CardPrefabResorceLoader.Instance.loadTextAsset("_Basic/description");
		}
		description.text = text.text;
	}

	private void displayCardReloadTime() {
		cardReloadTime.text = cardRepresented.basicAttrabutes.cardReloadTime_seconds.ToString();
	}

	private void displayCardsAlowedInDeck() {
		cardsAlowedInDeck.text = cardRepresented.basicAttrabutes.numberOfCardAllowedInDeck.ToString();
	}
}
