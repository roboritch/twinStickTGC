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

	#region pitch change
	private float basePitch;
	[SerializeField]
	private bool randomizePitch;
	[SerializeField]
	private float pitchChangeUpperBound = 1.10f;
	[SerializeField]
	private float pitchChangeLowerBound = 0.90f;

	private void pitchChange() {
		if(randomizePitch)
			sfxSorce.pitch = Random.Range(pitchChangeLowerBound+basePitch, pitchChangeUpperBound+basePitch);
	}
	#endregion

	void Awake () {
		sfxSorce = GetComponent<AudioSource>();
		basePitch = sfxSorce.pitch-1;
		baseSfxVolume = sfxSorce.volume;
		SoundLevels.Instance.setNewSfxCallback(this,updateSfxVolume);
		pitchChange();
    }

}
