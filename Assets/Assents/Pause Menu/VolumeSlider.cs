using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

	[SerializeField]
	private bool musicSlider;

	void Start() {
		if (musicSlider) {
			GetComponent<Slider>().value = SoundLevels.Instance.getMusicVolume();
		} else {
			GetComponent<Slider>().value = SoundLevels.Instance.getSfxVolume();
		}
	}

	/// <summary>
	/// call the singlton and update volume levels
	/// </summary>
	/// <param name="newVolume"></param>
	public void updateVolume(float newVolume) {
		SoundLevels.Instance.setMusicVolume(newVolume);
	}

	public void updateSfxVolume(float newVolume) {
		SoundLevels.Instance.setSfxVolume(newVolume);
	}

}

