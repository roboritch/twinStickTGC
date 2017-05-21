using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseStatsPanel : MonoBehaviour {
	[SerializeField]
	private GameObject hiddenObject;
	[SerializeField]
	private RectTransform resizingRect;
	[SerializeField]
	private RectTransform resizingTarget;

	[SerializeField]
	private bool active = true;

	private float activeSize;
	public void toggle() {
		if(active) {
			active = !active;
			hiddenObject.SetActive(false);
			activeSize = resizingRect.sizeDelta.x;
			resizingRect.sizeDelta = new Vector2(resizingTarget.sizeDelta.x, resizingRect.sizeDelta.y);
		} else {
			active = !active;
			resizingRect.sizeDelta = new Vector2(activeSize, resizingRect.sizeDelta.y);
			hiddenObject.SetActive(true);
		}
	}




}
