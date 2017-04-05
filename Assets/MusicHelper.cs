﻿using System.Collections;
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
	private float currentTime;

	// Use this for initialization
	void Awake() {
		player = GetComponent<AudioSource>();
	}

	void Start() {
		player.pitch = pitchAndPlaybackSpeed;
		player.time = startTime_seconds;
		endTime_seconds = player.clip.length - endTime_secondsBeforeClipEnd;
	}
	
	// Update is called once per frame
	void Update () {
		if(loop)
		if(player.time > endTime_seconds) { // VERY rough music looping
			player.time = startTime_seconds; 
		}
		currentTime = player.time;
	}
}
