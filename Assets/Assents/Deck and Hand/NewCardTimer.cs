using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NewCardTimer : MonoBehaviour {

	private float initalTime_sec = 1f;
	private float time_sec = 1f;
	private bool countingDown = false;

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
		if (countingDown && frameUpdate) {
			time_sec -= Time.deltaTime * Time.timeScale;
		}
		updateGrapic();
	}

	#region Grapics
	private void initGrapics() {
		grapicSize = GetComponent<RectTransform>();
		image = GetComponent<Image>();
	}

	private float barSize = 100f;
	private RectTransform grapicSize;
	private Image image;
	private void updateGrapic() {
		grapicSize.sizeDelta = new Vector2(barSize * (time_sec / initalTime_sec), grapicSize.sizeDelta.y);
	}

	public enum ColorsOfCardTimer {
		Basic,Slowed,SpeadUp,Stoped
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

	// Use this for initialization
	void Start () {
		initGrapics();
    }
	
	// Update is called once per frame
	void Update () {
		updateTimer(true);
    }
}
