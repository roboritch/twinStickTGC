using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

public delegate void KeypressCallback();

/// <summary>
/// when added to the game this crates key bindings that can be changed mid game
/// </summary>
public class KeyEvents : Singleton<KeyEvents>{
	
	[SerializeField] private string localFileName = "UserBindings";
	[SerializeField] private string localFolderName = "User Preferences";
	private string filePath(string nameAddendum) {
		return localFolderName + "/" + localFileName + nameAddendum;
	}
	[SerializeField] private bool loadBindingsFromFile = false;
	void Awake(){
		initDefaultKeys(false,"");
	}

	#region Mouse control
	public void constrainMouseToScreen(bool state) {
		if(state)
		Cursor.lockState = CursorLockMode.Confined;
		else
		Cursor.lockState = CursorLockMode.None;
	}
	#endregion

	#region Dictionary 
	/// <summary>
	/// set on program initialization not changed after that 
	/// </summary>
	private Dictionary<string, KeySetting> keySettings;
	/// <summary>
	/// set at runtime if there are any additional keys the program uses
	/// not saved, any user key requirements must be saved 
	/// </summary>
	private Dictionary<string, KeySetting> dynamicKeys;

	private void initDefaultKeys(bool tryLoadUserSettings,string nameAddendum) {
		dynamicKeys = new Dictionary<string, KeySetting>();
		if(tryLoadUserSettings) {
			if(!SaveAndLoadJson.LoadDictionary(filePath(nameAddendum),out keySettings)) {
				Debug.Log("could not load settings, using hard coded defaults");
			} else {
				return;
			}
		}

		//these keys must match up with other parts of the program using them
		//these settings will eventually be loaded from a users settings 
		keySettings = new Dictionary<string, KeySetting> {
			{ "mainAction",	new KeySetting(	new KeyCode[]	{ KeyCode.E },		KeypressState.buttonDown) },

			{ "moveUp",		new KeySetting(	new KeyCode[]	{ KeyCode.W },		KeypressState.buttonHeld) },
			{ "moveDown",	new KeySetting(	new KeyCode[]	{ KeyCode.S },		KeypressState.buttonHeld) },
			{ "moveLeft",	new KeySetting(	new KeyCode[]	{ KeyCode.A },		KeypressState.buttonHeld) },
			{ "moveRight",	new KeySetting(	new KeyCode[]	{ KeyCode.D },		KeypressState.buttonHeld) },

			{ "card1",		new KeySetting(	new KeyCode[]	{ KeyCode.Alpha1 },	KeypressState.buttonDown) },
			{ "card2",		new KeySetting(	new KeyCode[]	{ KeyCode.Alpha2 },	KeypressState.buttonDown) },
			{ "card3",		new KeySetting(	new KeyCode[]	{ KeyCode.Alpha3 },	KeypressState.buttonDown) },
			{ "card4",		new KeySetting(	new KeyCode[]	{ KeyCode.Alpha4 },	KeypressState.buttonDown) },
		};
	}

	#region user modification
	private Dictionary<string, KeySetting> tempSettings = new Dictionary<string, KeySetting>();
	/// <summary>
	/// this should be called by a key setting GUI
	/// </summary>
	public void resetOrCancelNewSettings() {
		tempSettings = keySettings;
	}

	/// <summary>
	/// this allows a GUI to iterate through the current tempSettings
	/// to get the names and values of the key settings
	/// </summary>
	/// <returns>all the key names and settings</returns>
	public KeyValuePair<string,KeySetting>[] getAllCurrentSettings() {
		return tempSettings.ToArray();
	}

	/// <summary>
	/// this is only on the temp dictionary
	/// to save changes be sure to apply changes
	/// </summary>
	/// <param name="name">key setting name</param>
	/// <param name="newSettings">basic settings (callbacks are carnied over from original settings)</param>
	public void setNewKeySetting(string name,KeySetting newSettings) {
		KeySetting setting;
		if(!tempSettings.TryGetValue(name, out setting)) {
			Debug.LogError("no setting with that name," +
				"new keys must be set on plication startup" +
				"or use the dynamic key settings and there own saves");
		} else {
			setting.keys = newSettings.keys;
			setting.mouse = newSettings.mouse;
			setting.pressState = newSettings.pressState;
			//callbacks are left the way they are
		}
	}

	/// <summary>
	/// get a key from tempSettings
	/// </summary>
	/// <param name="keySettingName"></param>
	/// <returns></returns>
	public KeySetting getNewKeySetting(string keySettingName) {
		KeySetting setting;
		if(!tempSettings.TryGetValue(keySettingName, out setting)) {
			Debug.LogError("get key failed");
			return null;
		}
		return setting;
	}


	/// <summary>
	/// make tempSettings the current settings
	/// </summary>
	/// <param name="nameAddendum">many Settings can be saved
	/// with slightly differing names leave empty if there is only one setting</param>
	/// <returns>save to disk worked or not</returns>
	public bool setAndSaveKeySettings(string nameAddendum) {
		keySettings = tempSettings;
		bool worked = SaveAndLoadJson.saveDictionary(filePath(nameAddendum), keySettings);
		if(!worked) {
			Debug.LogError("keySettings not saved!");
		}
		return worked;
	}
	#endregion

	#region set and remove dynamicKeys
	/// <summary>
	/// add a key setting
	/// add any callbacks to the KeySetting if you want them
	/// </summary>
	/// <param name="keyName"></param>
	/// <param name="keySetting"></param>
	public void addKeySetting(string keyName, KeySetting keySetting) {
		if(dynamicKeys.ContainsKey(keyName)) {
			Debug.Log("key name already exists, adding callbacks from this setting to the current one");
			KeySetting existingSetting;
			dynamicKeys.TryGetValue(keyName, out existingSetting);
			//remove the extra empty callback 
			existingSetting.callbacks += keySetting.callbacks - emptyCallback;
		} else {
			dynamicKeys.Add(keyName, keySetting);
		}
	}

	/// <summary>
	/// remove a key setting
	/// WARNING this should not be used unless certain
	/// there is nothing else using the setting
	/// </summary>
	/// <param name="keyName"></param>
	public void removeKeySetting(string keyName) {
		if(!dynamicKeys.Remove(keyName)) {
			Debug.LogWarning("key setting " + keyName + "does not currently exist");
		}
	}

	#endregion

	#region set and remove callbacks 
	/// <summary>
	/// add a callback to a callbackName
	/// callbackName must already exist
	/// use dynamicKeys for temporary keys
	/// </summary>
	/// <param name="callbackName"></param>
	/// <param name="callback"></param>
	public void setCallback(string callbackName, KeypressCallback callback) {
		KeySetting setting;
		if(keySettings.TryGetValue(callbackName, out setting)){ // get the settings for that key name
			setting.callbacks += callback; //add the callback to the list of callbacks
		} else {
			Debug.LogWarning("set: no key binding found with the name " + callbackName);
		}
	}

	public void removeCallback(string callbackName, KeypressCallback callback) {
		KeySetting setting;
		if(keySettings.TryGetValue(callbackName, out setting)) {
			try {
				setting.callbacks -= callback; //WARNING untested
			} catch(System.Exception) {
				Debug.LogWarning("that callback is not found in " + callbackName);
			}
		} else {
			Debug.LogWarning("remove: no key binding found with the name " + callbackName);
		}
	}
	#endregion

	#endregion

	#region action delegates
	/// <summary>
	/// used to prevent reference errors in case no actions are set to a button
	/// </summary>
	public void emptyCallback() {
		
	}

	#endregion

	private void Update() {
		checkAllKeys();
	}
	
	#region key checking methods
	private void checkAllKeys() {
		//iterate through all basic set keys
		foreach(KeyValuePair<string, KeySetting> item in keySettings) {
			checkSingleKeySet(item.Value);
		}

		//this section would be vulnerable to dictionary iteration errors
		//if a callback removed the dynamic refinance
		KeySetting[] listOfDynamicKeys = dynamicKeys.Values.ToArray();
		for(int i = 0; i < listOfDynamicKeys.Length; i++) {
			checkSingleKeySet(listOfDynamicKeys[i]);
		}
	}

	private void checkSingleKeySet(KeySetting setting) {
		//check if key uses mouse 
		if(setting.mouse >= 0) {
			switch(setting.pressState) {
				case KeypressState.buttonDown:
				if(Input.GetMouseButtonDown(setting.mouse)) {
					setting.callbacks();
				}
				break;
				case KeypressState.buttonUp:
				if(Input.GetMouseButtonUp(setting.mouse)) {
					setting.callbacks();
				}
				break;
				case KeypressState.buttonHeld:
				if(Input.GetMouseButton(setting.mouse)) {
					setting.callbacks();
				}
				break;
			}
			return;
		}

		//simplified for one key
		if(setting.keys.Length == 1) {
			if(getKeyState(setting.keys[0], setting.pressState)) {
				setting.callbacks();
			}
			return;
		}

		bool someButtonIsJustPressed = false;
		bool someButtonIsUp = false;

		for(int i = 0; i < setting.keys.Length; i++) {
			//both press states are used for accuracy, if additional performance is required use just buttonHeld
			if(getKeyState(setting.keys[i], KeypressState.buttonHeld)) {
				//no action required
			} else if(getKeyState(setting.keys[i], KeypressState.buttonDown)) {
				someButtonIsJustPressed = true;
			} else if(getKeyState(setting.keys[i], KeypressState.buttonUp)) {
				someButtonIsUp = true;
			} else {
				return;
			}
		}

		//if gotten here all keys required are down held or up

		if(setting.pressState == KeypressState.buttonDown) {
			//this will only activate on the frame all buttons in this setting are pressed	
			if(someButtonIsJustPressed) {
				setting.callbacks();
			} else {
				return;
			}
		} else if(setting.pressState == KeypressState.buttonUp) {
			//this will only activate on the frame all buttons in this setting are held/pressed and at least one is up
			if(someButtonIsUp) {
				setting.callbacks();
			} else {
				return;
			}
		} else if(setting.pressState == KeypressState.buttonHeld) {
			if(someButtonIsUp == false) { // held state ends 1 frame earlier (more accurate)
				//also called if all buttons are pressed together on the same frame
				setting.callbacks();
			}
		}
	}

	/// <summary>
	/// get key's current state
	/// </summary>
	/// <param name="code"></param>
	/// <param name="setting"></param>
	/// <returns></returns>
	private bool getKeyState(KeyCode code,KeypressState setting) {
		switch(setting) {
			case KeypressState.buttonDown:
			return Input.GetKeyDown(code);
			case KeypressState.buttonUp:
			return Input.GetKeyUp(code);
			case KeypressState.buttonHeld:
			return Input.GetKey(code);
			default:
			Debug.LogWarning("invalid KeypressState setting");
			return false;
		}
	}

	#endregion

}


[System.Serializable]
public class KeySetting {
	public KeySetting(KeyCode[] keyRequirements, KeypressState state){
		mouse = -1;
		keys = keyRequirements;
		pressState = state;
		callbacks = emptyCallbeck;
	}

	public KeySetting(int mouseButton, KeypressState state) {
		if(mouseButton < 0 || mouseButton > 3) {
			Debug.Log("error, not valid mouse button");
		}
		mouse = mouseButton;
		keys = new KeyCode[0];
		pressState = state;
		callbacks = emptyCallbeck;
	}

	public static void emptyCallbeck() {

	}

	///this struct can be used to handle mouse buttons
	public int mouse;
	///all the keys required
	public KeyCode[] keys;
	///for multiple keys the press state only effects the final key-press
	///all individual key checks are done using on button down
	public KeypressState pressState;
	///this var is to be set by the various scripts at runtime
	[System.NonSerialized]
	public KeypressCallback callbacks;
}

public enum KeypressState : int {
	buttonDown,
	buttonUp,
	buttonHeld
}

//old implementation archive 
#if false
//use example as format for saved keys
[System.Serializable]
public struct ActionBindings{
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("mainAction")]
	public KeyCode[] mainAction;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("moveUp")]
	public KeyCode[] moveUp;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("moveDown")]
	public KeyCode[] moveDown;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("moveLeft")]
	public KeyCode[] moveLeft;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("moveRight")]
	public KeyCode[] moveRight;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("activateCard1")]
	public KeyCode[] activateCard1;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("activateCard2")]
	public KeyCode[] activateCard2;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("activateCard3")]
	public KeyCode[] activateCard3;
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("activateCard4")]
	public KeyCode[] activateCard4;
}

	
public struct delegateCallbacks {
	public KeypressCallback mainAction; //can store multuple methods to call when the button is pressed
	public KeypressCallback moveUp;
	public KeypressCallback moveDown;
	public KeypressCallback moveLeft;
	public KeypressCallback moveRight;
	public KeypressCallback[] activateCard;
}

	//key events must be manually placed in the update
	//can be done with deletes be this is easer to debug
	//with all elements visible
#region button checks
	//TODO put this in a dictionary
	void Update(){
		if (correctKeysPressed(currentActionBindings.mainAction)) {
			buttonCallbackFunctions.mainAction();
		}
		if (correctKeysPressed(currentActionBindings.moveUp)) {
			buttonCallbackFunctions.moveUp();
		}
		if (correctKeysPressed(currentActionBindings.moveDown)) {
			buttonCallbackFunctions.moveDown();
		}
		if (correctKeysPressed(currentActionBindings.moveLeft)) {
			buttonCallbackFunctions.moveLeft();
		}
		if (correctKeysPressed(currentActionBindings.moveRight)) {
			buttonCallbackFunctions.moveRight();
		}
		if (correctKeysPressed(currentActionBindings.activateCard1)) {
			buttonCallbackFunctions.activateCard[0]();
		}
		if (correctKeysPressed(currentActionBindings.activateCard2)) {
			buttonCallbackFunctions.activateCard[1]();
		}
		if (correctKeysPressed(currentActionBindings.activateCard3)) {
			buttonCallbackFunctions.activateCard[2]();
		}
		if (correctKeysPressed(currentActionBindings.activateCard4)) {
			buttonCallbackFunctions.activateCard[3]();
		}
	}

	/// <summary>
	///key callbacks as information for correctKeysPressed
	/// F9  == button down
	/// F10 == button held
	/// F11 == button up
	/// F12 == 2 keys, first held second down
	/// </summary>
	/// <param name="modifyer"></param>
	/// <param name="inputCode"></param>
	private bool getInputKeyInfo(KeyCode modifyer,KeyCode inputCode) {
		switch (modifyer) {
			case KeyCode.F9:
				return Input.GetKeyDown(inputCode);
			case KeyCode.F10:
				return Input.GetKey(inputCode);
			case KeyCode.F11:
				return Input.GetKeyUp(inputCode);
			default:
				return false;

		}
#endregion

	public void tryToLoadBidings(){
		ActionBindings tempAB; 
		if(!SaveAndLoadXML.loadXML<ActionBindings>(appendToLocalPath(localFolderName + "/" + localFileName), out tempAB)){
			Debug.LogWarning("XML bindings load failure");
			return;
		}
		currentActionBindings = tempAB;
	}

	private string appendToLocalPath(string filePathAndName){
		return Application.dataPath + "/" + filePathAndName;
	}

	public ActionBindings getActionBindings(){
		return currentActionBindings;
	}

		/*	instructions to assign a method to a KeypressCallback 
		nameOfDelegateContainer += methodName;
		the plus is necessary since multiple methods classes may want to work with the same callback
		*/
	public delegateCallbacks buttonCallbackFunctions;

		/// <summary>
	/// all default keys must be set here in lew of a key changing screen
	/// </summary>probabilityMultiplyer_4Point
	private void setDefaultBindings(){
		currentActionBindings.mainAction = new KeyCode[] {KeyCode.F9, KeyCode.E};
		buttonCallbackFunctions.mainAction = emptyCallback;

		currentActionBindings.moveUp = new KeyCode[] { KeyCode.F10, KeyCode.W };
		buttonCallbackFunctions.moveUp = emptyCallback;

		currentActionBindings.moveDown = new KeyCode[] { KeyCode.F10, KeyCode.S };
		buttonCallbackFunctions.moveDown = emptyCallback;

		currentActionBindings.moveLeft = new KeyCode[] { KeyCode.F10, KeyCode.A };
		buttonCallbackFunctions.moveLeft = emptyCallback;

		currentActionBindings.moveRight = new KeyCode[] { KeyCode.F10, KeyCode.D };
		buttonCallbackFunctions.moveRight = emptyCallback;

		buttonCallbackFunctions.activateCard = new KeypressCallback[4];

		currentActionBindings.activateCard1 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha1 };
		buttonCallbackFunctions.activateCard[0] = emptyCallback;

		currentActionBindings.activateCard2 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha2 };
		buttonCallbackFunctions.activateCard[1] = emptyCallback;

		currentActionBindings.activateCard3 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha3 };
		buttonCallbackFunctions.activateCard[2] = emptyCallback;

		currentActionBindings.activateCard4 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha4 };
		buttonCallbackFunctions.activateCard[3] = emptyCallback;
	}
#endif


