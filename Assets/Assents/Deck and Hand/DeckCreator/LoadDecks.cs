using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class LoadDecks : MonoBehaviour {

	[SerializeField]
	private DeckList decklist;

	private Dropdown dropdown;
	private TextAsset[] decks;

	public void loadSelectedDeck() {
		decklist.loadDeck(decks[dropdown.value].name); 
	}

	private void Awake() {
		dropdown = GetComponent<Dropdown>();
	}

	// Use this for initialization
	void Start () {
		loadDeckFolder("Player Decks");
	}
	
	public void loadDeckFolder(string folderName) {
		decks = Resources.LoadAll<TextAsset>(folderName);
		dropdown.ClearOptions();
		List<Dropdown.OptionData> dropdownData = new List<Dropdown.OptionData>();
		for(int i = 0; i < decks.Length; i++) {
			dropdownData.Add(new Dropdown.OptionData(decks[i].name));
		}
		dropdown.AddOptions(dropdownData);
	}
	
	public void loadDeckType(Toggle deckType) {
		if(deckType.isOn) {
			loadDeckFolder(Deck.playerDecks.TrimEnd('/'));
		} else {
			loadDeckFolder(Deck.baddyDecks.TrimEnd('/'));
		}
	}

}
