using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : MonoBehaviour {

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

