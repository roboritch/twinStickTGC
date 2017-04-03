using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioControler : MonoBehaviour {
	private AudioSource audioSorce;
	private AudioClip audioClip;

	public void playAudio() {
		//TODO play
	}



	void Awake() {
		audioSorce = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
