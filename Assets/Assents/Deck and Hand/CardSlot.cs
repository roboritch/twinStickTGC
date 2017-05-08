using System;
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

	public void removeCard() {
		cardBeingHeld.destroyCard();
		cardBeingHeld = null;
	}

	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private Sprite noCardsLeft;

	public void displayCardIcon() {
		if (cardBeingHeld != null)
			image.sprite = CardPrefabResorceLoader.Instance.loadSprite(cardBeingHeld.basicAttrabutes.cardIconPath);
		else
			image.sprite = noCardsLeft;
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
		if(cardBeingHeld.useCard(hand.handUser)) { //card activation success
			startCountdown(cardBeingHeld.basicAttrabutes.cardReloadTime_seconds);
			displayDefaultSprite();
			cardBeingHeld.destroyCard();
			cardBeingHeld = null;
		}
	}

	void Awake() {
		hand = GetComponentInParent<Hand>();
		initCardSlot();
	}

	// Use this for initialization
	void Start () {
		initTimerGrapics();
    }
	
	// Update is called once per frame
	void Update () {
		if (countingDown) {
			updateTimer(true);
		}
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
	/// updates countdown
	/// </summary>
	/// <param name="frameUpdate">true if called in NewCardTimer's Update method, false otherwise</param>
	private void updateTimer(bool frameUpdate) {
		if (frameUpdate) {
			time_sec -= Time.deltaTime * Time.timeScale;
		}
		checkTimerDone();
		updateGrapic_Timer();
	}

	private void checkTimerDone() {
		if (time_sec <= 0f) {
			countingDown = false;
			drawCard();
		}
	}

	private void drawCard() {
		Card card;
		if (CardConstructor.constructCard(hand.deck.drawCard(), out card)) {
			receiveCard(card);
		} else {
			Debug.LogWarning("no cards left in deck");
			displayCardIcon();
		}
		resetTimerGrapics();
		updateGrapic_Timer();
	}
	#endregion


	#region Graphics
	private void initTimerGrapics() {
		grapicSize = transform.GetChild(0).GetComponent<RectTransform>();
		barSize = grapicSize.sizeDelta.x;

		timerImage = transform.GetChild(0).GetComponent<Image>();
	}

	private void resetTimerGrapics() {
		initalTime_sec = 1f;
		time_sec = 1f;
	}

	private float barSize = 100f;
	private RectTransform grapicSize;
	private Image timerImage;
	private void updateGrapic_Timer() {
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