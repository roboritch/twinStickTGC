using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicHelper : MonoBehaviour {
	private AudioSource player;

	[SerializeField]
	private float pitchAndPlaybackSpeed;
	[SerializeField]
	private float startTime_seconds;
	[SerializeField]
	private float endTime_secondsBeforeClipEnd;
	private float endTime_seconds;
	[SerializeField]
	private bool loop;
	[SerializeField]
	private bool fadein;
	[SerializeField]
	private float currentTime;

	// Use this for initialization
	void Awake() {
		player = GetComponent<AudioSource>();
	}

	void Start() {
		player.pitch = pitchAndPlaybackSpeed;
		player.time = startTime_seconds;
		endTime_seconds = player.clip.length - endTime_secondsBeforeClipEnd;

		if (fadein) {
			player.time -= -5f;
			if(player.time <= 0) {
				player.time = 0f;
			}
			fadinEndTime_seconds = 4f;
			fadeinEndVolume = player.volume;
			player.volume = 0f;
		}

	}

	private float fadeinEndVolume;
	private float currentFadeTime_seconds = 0f;
	private float fadinEndTime_seconds;
	// Update is called once per frame
	void Update () {
		if (fadein) {
			currentFadeTime_seconds += Time.deltaTime;
            player.volume = fadeinEndVolume * (currentFadeTime_seconds/fadinEndTime_seconds);
			if(currentFadeTime_seconds > fadinEndTime_seconds) {
				fadein = false;
			}
		}

		if(loop)
		if(player.time > endTime_seconds) { // VERY rough music looping
			player.time = startTime_seconds; 
		}
		currentTime = player.time;
	}
}
