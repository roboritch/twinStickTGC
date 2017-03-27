using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlash : MonoBehaviour {


	private SpriteRenderer sr;
	public void flashSprite(float inTime_sec) {
		spriteStateChangeTimer_seconds = inTime_sec;
		running = true;
	}

	[SerializeField]
	private bool destoryWhenDone;

	private bool running = false;
	private float spriteStateChangeTimer_seconds;

	[SerializeField] private float flashRate_seconds = 0.2f;

	[SerializeField]
	private float flashingLength_seconds = 1.0f;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			if(spriteStateChangeTimer_seconds <= 0) {
				sr.enabled = !sr.enabled;
				spriteStateChangeTimer_seconds = flashRate_seconds;
			} else {
				spriteStateChangeTimer_seconds -= Time.deltaTime;
			}
			flashingLength_seconds -= Time.deltaTime;
			if(flashingLength_seconds <= 0) {
				running = false;
				if (destoryWhenDone) {
					UnityExtentionMethods.destoryAllChildren(transform);
				}
			}
		}
	}
}
