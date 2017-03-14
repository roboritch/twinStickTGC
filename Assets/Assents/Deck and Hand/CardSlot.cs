﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardSlot : MonoBehaviour {
	private Image image;

	[SerializeField] private Card cardBeingHeld;
	public void receiveCard(Card card) {
		cardBeingHeld = card;
		displayCardIcon();
	}


	[SerializeField]
	private Sprite defaultSprite;
	public void displayCardIcon() {
		if(cardBeingHeld != null)
		image.sprite = cardBeingHeld.cardArt;
	}
	
	private void displayDefaultSprite() {
		image.sprite = defaultSprite;
	}

	private void initCardSlot() {
		image = GetComponent<Image>();
	}
	
	private Hand hand;
	/// <summary>
	/// this is called on a button press
	/// </summary>
	public void activateCard() {
		if(cardBeingHeld != null)
		if(cardBeingHeld.useCard(hand.curretPlayer)) { //card activation succsess
			startCountdown(cardBeingHeld.cardReloadTime_seconds);
			displayDefaultSprite();
			cardBeingHeld = null;
		}
	}

	// Use this for initialization
	void Start () {
		hand = GetComponentInParent<Hand>();
		initCardSlot();
		initTimerGrapics();
    }
	
	// Update is called once per frame
	void Update () {
		if(countingDown)
		updateTimer(true);
	}

	#region New Card Timer

	private float initalTime_sec = 1f;
	private float time_sec = 1f;
	private bool countingDown = false;


	#region Timer updates
	public void startCountdown(float timeTillNewCard_sec) {
		time_sec = timeTillNewCard_sec;
		initalTime_sec = timeTillNewCard_sec;
		countingDown = true;
	}

	public void reduceCountDown(float timeReduced_sec) {
		time_sec -= timeReduced_sec;
		updateTimer(false);
	}

	/// <summary>
	/// updates countown
	/// </summary>
	/// <param name="frameUpdate">true if called in NewCardTimer's Update method, false otherwise</param>
	private void updateTimer(bool frameUpdate) {
		if (frameUpdate) {
			time_sec -= Time.deltaTime * Time.timeScale;
		}
		checkTimerDone();
		updateGrapic();
	}

	private void checkTimerDone() {
		if (time_sec <= 0f) {
			countingDown = false;
			drawCard();
		}
	}

	private void drawCard() {
		Type card = hand.deck.drawCard();
		if (card != null)
			receiveCard((Card)Activator.CreateInstance(card));
	}
	#endregion


	#region Grapics
	private void initTimerGrapics() {
		grapicSize = transform.GetChild(0).GetComponent<RectTransform>();
		timerImage = transform.GetChild(0).GetComponent<Image>();
	}

	private float barSize = 100f;
	private RectTransform grapicSize;
	private Image timerImage;
	private void updateGrapic() {
		if (initalTime_sec != 0)
			grapicSize.sizeDelta = new Vector2(barSize * (time_sec / initalTime_sec), grapicSize.sizeDelta.y);
	}

	public enum ColorsOfCardTimer {
		Basic, Slowed, SpeadUp, Stoped
	}
	private Color getColor(ColorsOfCardTimer col) {
		switch (col) {
			case ColorsOfCardTimer.Basic:
				return Color.white;
			case ColorsOfCardTimer.Slowed:
				return Color.gray;
			case ColorsOfCardTimer.SpeadUp:
				return Color.yellow;
			case ColorsOfCardTimer.Stoped:
				return Color.red;
			default:
				return Color.white;
		}
	}

	private void changeGrapicColor(ColorsOfCardTimer color) {
		image.color = getColor(color);
	}
	#endregion

	#endregion


}