using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class LoadDecks : MonoBehaviour {

	private Dropdown dropdown;
	private TextAsset[] decks;

	public void selectNewDeck(int deckValue) {

	}

	private void Awake() {
		dropdown = GetComponent<Dropdown>();
	}

	// Use this for initialization
	void Start () {
		decks =  Resources.LoadAll<TextAsset>("Player Decks");
		dropdown.ClearOptions();
		List<Dropdown.OptionData> dropdownData = new List<Dropdown.OptionData>();
		for(int i = 0; i < decks.Length; i++) {
			dropdownData.Add(new Dropdown.OptionData(decks[i].name));
		}
		dropdown.AddOptions(dropdownData);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
