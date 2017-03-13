using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardSlot : MonoBehaviour {
	private Image image;

	public Card CardBeingHeld;

	[SerializeField]
	private Sprite defaultSprite;
	public void displayCardIcon() {
		image.sprite = CardBeingHeld.cardArt;
	}
	
	private void displayDefaultSprite() {
		image.sprite = defaultSprite;
	}

	private void initCardSlot() {
		image = GetComponent<Image>();
	}

	// Use this for initialization
	void Start () {
		initCardSlot();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
