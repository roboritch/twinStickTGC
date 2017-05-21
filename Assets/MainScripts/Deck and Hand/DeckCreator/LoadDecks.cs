using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class LoadDecks : MonoBehaviour {

	[SerializeField]
	private DeckList decklist;

	private Dropdown dropdown;
	private DeckStorage[] decks;

	public void loadSelectedDeck() {
		decklist.loadDeck(decks[dropdown.value].name); 
	}

	private void Awake() {
		dropdown = GetComponent<Dropdown>();
	}

	// Use this for initialization
	void Start () {
		loadDeckFolder(SaveAndLoadJson.getBaseFilePath(Deck.playerDecks));
	}
	
	public void loadDeckFolder(string folderPath) {
		string[] deckPathList;
		try {
			deckPathList = Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly);
		} catch(System.Exception) {
			Debug.LogError("error loading deck list");
			return;
		}
		

		if(deckPathList == null ) {
			Debug.LogError("error loading deck list");
			return;
		}
		if(deckPathList.Length == 0) {
			Debug.LogError("no decks found at " + folderPath);
			return;
		}

		decks = new DeckStorage[deckPathList.Length];
		for(int i = 0; i < deckPathList.Length; i++) {
			decks[i] = new DeckStorage(Path.GetFileNameWithoutExtension(deckPathList[i]), File.ReadAllText(deckPathList[i]));
		}
	
		dropdown.ClearOptions();
		List<Dropdown.OptionData> dropdownData = new List<Dropdown.OptionData>();
		for(int i = 0; i < decks.Length; i++) {
			dropdownData.Add(new Dropdown.OptionData(decks[i].name));
		}
		dropdown.AddOptions(dropdownData);
	}
	
	//TODO abstract which type is loaded at start
	public void loadDeckType(Toggle deckType) {
		if(deckType.isOn) {
			loadDeckFolder(SaveAndLoadJson.getBaseFilePath(Deck.playerDecks));
		} else {
			loadDeckFolder(SaveAndLoadJson.getResourcePath(Deck.baddyDecks));
		}
	}
	
	private struct DeckStorage {
		public DeckStorage(string name, string contents) {
			this.name = name;
			jsonDeck = contents;

		}

		public string name;
		public string jsonDeck;
	}

}

