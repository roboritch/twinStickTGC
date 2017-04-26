using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundLevels : Singleton<SoundLevels> {

	[SerializeField]
	private float musicVolume = 1f;
	[SerializeField]
	private float sfxVolume = 1f;

	public float getMusicVolume() {
		return musicVolume;
	}

	public float getSfxVolume() {
		return sfxVolume;
	}

	public void setMusicVolume(float volume) {
		musicVolume = volume;
		notifyMusicCallbacks();
	}

	public void setSfxVolume(float volume) {
		sfxVolume = volume;
		notifySfxCallbacks();
	}


	public delegate void soundVolumeChange(float newVolume);
	
	#region Music Callbacks
	private Dictionary<object, soundVolumeChange> musicChangeCallbacks = new Dictionary<object, soundVolumeChange>(10);
	/// <summary>
	/// this method should only be called once for one object
	/// this method calls the volumeCallbackMethod instantly
	/// </summary>
	/// <param name="callbackDestination">the origin of the volumeCallbackMethod </param>
	/// <param name="volumeCallbackMethod">method to call when volume is changed</param>
	public void setNewMusicCallback(object callbackDestination, soundVolumeChange volumeCallbackMethod) {
		musicChangeCallbacks.Add(callbackDestination, volumeCallbackMethod);
		volumeCallbackMethod(musicVolume);
	}

	private void notifyMusicCallbacks() {
		foreach (var method in musicChangeCallbacks) {
			method.Value(musicVolume);
		}
	}

	/// <summary>
	/// this can be automated by checking if obj referenced no longer exists
	/// </summary>
	public void removeMusicCallback(object callbackDestination) {
		musicChangeCallbacks.Remove(callbackDestination);
	}
	#endregion

	#region SFX Callbacks
	private Dictionary<object, soundVolumeChange> sfxChangeCallbacks = new Dictionary<object, soundVolumeChange>(10);
	/// <summary>
	/// this method should only be called once for one object
	/// </summary>
	/// <param name="callbackDestination">the origin of the volumeCallbackMethod </param>
	/// <param name="volumeCallbackMethod">method to call when volume is changed</param>
	public void setNewSfxCallback(object callbackDestination, soundVolumeChange volumeCallbackMethod) {
		sfxChangeCallbacks.Add(callbackDestination, volumeCallbackMethod);
		volumeCallbackMethod(getSfxVolume());
    }

	private void notifySfxCallbacks() {
		foreach (var method in sfxChangeCallbacks) {
			method.Value(musicVolume);
		}
	}

	/// <summary>
	/// this can be automated by checking if obj referenced no longer exists
	/// </summary>
	public void removeSfxCallback(object callbackDestination) {
		sfxChangeCallbacks.Remove(callbackDestination);
	}
	#endregion
	
	// Use this for initialization
	void Awake () {
		loadSoundLevels();
	}
	
	void Start() {
		Invoke("notifyMusicCallbacks", 0.01f);
		Invoke("notifySfxCallbacks", 0.01f);
	}

	// Update is called once per frame
	void Update () {
		
	}

	#region xml saving for sound levels
	private string soundFilePath() {
		return SaveAndLoadXML.getBaseFilePath() + folderName + "/" + fileName;
    }

	private string fileName = "Sound Levels.xml";
	private string folderName = "User Preferences";
	private void loadSoundLevels() {
		SoundLevelsDiskSave loadedLevels;
		bool fileExists = SaveAndLoadXML.loadXML(soundFilePath(), out loadedLevels);
		if (fileExists) {
			setMusicVolume(loadedLevels.musicLevel);
			setSfxVolume(loadedLevels.sfxLevel);
		}
	}

	private void saveSoundLevels() {
		SoundLevelsDiskSave savingFile = new SoundLevelsDiskSave(musicVolume, sfxVolume);
		SaveAndLoadXML.saveXML(soundFilePath(), savingFile);
	}
	#endregion

	new void OnDestroy() {
		base.OnDestroy();
		saveSoundLevels();
	}
}

public struct SoundLevelsDiskSave {
	public SoundLevelsDiskSave(float music,float sfx) {
		musicLevel = music;
		sfxLevel = sfx;
	}

	[XmlElement("Music_Level")]
	public float musicLevel;
	[XmlElement("Sfx_Level")]
	public float sfxLevel;
}

