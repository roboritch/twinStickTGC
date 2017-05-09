using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBar : MonoBehaviour {



	private bool visible = true;


	public void toggleVisible() {
		if(visible) {
			hideParent();
		} else {
			showParent();
		}
	}


	private float lastAnchoredPosition = -15161564;
	private float lastSizeDelta = - -15161564;

	public void showParent() {
		if(lastAnchoredPosition == -15161564 || lastSizeDelta == -15161564) {
			return;
		}

		visible = true;
		RectTransform trans = transform.parent.GetComponent<RectTransform>();
		trans.anchoredPosition = new Vector2(lastAnchoredPosition, trans.anchoredPosition.y);
		trans.sizeDelta = new Vector2(lastSizeDelta, trans.sizeDelta.y);
	}

	public void hideParent() {
		visible = false;
		RectTransform trans = transform.parent.GetComponent<RectTransform>();
		lastAnchoredPosition = trans.anchoredPosition.x;
		lastSizeDelta = trans.sizeDelta.x;

		trans.anchoredPosition = new Vector2(0, trans.anchoredPosition.y);
		trans.sizeDelta = new Vector2(0, trans.sizeDelta.y);
	}


}
