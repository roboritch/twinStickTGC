using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAllCards : MonoBehaviour {

	[SerializeField]
	private GameObject cardDisplayPrefab;

	private void InitCardSelector() {
		for(int i = 0; i < transform.childCount; i++) {
			UnityExtentionMethods.destoryAllChildren(transform.GetChild(i));
		}

		IEnumerable<Card> allCards = ReflectiveEnumerator.GetEnumerableOfType<Card>();
		foreach(Card card in allCards) {
			//card base classes with this class name are not real cards and should be ignored
			if(card.GetType().Name.Contains("Base_")) {
				continue;
			}
			Instantiate(cardDisplayPrefab, transform);
		}

	}




	// Use this for initialization
	void Start () {
		InitCardSelector();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
