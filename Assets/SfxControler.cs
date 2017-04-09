using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxControler : MonoBehaviour {

	private float baseSfxVolume;

	private AudioSource sfxSorce;
	public void updateSfxVolume(float newVolume) {
		sfxSorce.volume = baseSfxVolume * newVolume;
	}

	void Start () {
		sfxSorce = GetComponent<AudioSource>();
		baseSfxVolume = sfxSorce.volume;
		SoundLevels.Instance.setNewSfxCallback(this,updateSfxVolume);
	}

}
