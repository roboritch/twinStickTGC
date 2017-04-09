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
		musicTrackVolume = player.volume;

		if (fadein) {
			player.time -= -5f;
			if(player.time <= 0) {
				player.time = 0f;
			}
			fadinEndTime_seconds = 4f;
			musicTrackVolume = player.volume;
			player.volume = 0f;
		}

		initVolumeCallback(); // must be called after music track volume is set
	}

	#region Volume Callback

	public void updateVolume(float newVolume) {
		player.volume = newVolume * musicTrackVolume;
	}
	
	private void initVolumeCallback() {
		SoundLevels.Instance.setNewMusicCallback(this, updateVolume);
	}

	#endregion


	private float musicTrackVolume = 1f; // defalut volume is 1
	private float currentFadeTime_seconds = 0f;
	private float fadinEndTime_seconds;
	// Update is called once per frame
	void Update () {
		if (fadein) {
			currentFadeTime_seconds += Time.deltaTime;
            player.volume = musicTrackVolume * (currentFadeTime_seconds/fadinEndTime_seconds);
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
