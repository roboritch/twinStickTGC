﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCardResorces : MonoBehaviour {

	public void loadAllCardsResorces() {
		CardPrefabResorceLoader.Instance.preLoadAllCards();
	}


}
