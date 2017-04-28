using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Serialization;
using System.IO;


public delegate void KeypressCallback();

/// <summary>
/// when added to the game this crates key bindings that can be changed mid game
/// </summary>
public class KeyEvents : Singleton<KeyEvents>{
	
	[SerializeField] private string localFileName = "Generic Bindings";
	[SerializeField] private string localFolderName = "User Preferences";
	private ActionBindings currentActionBindings;
	[SerializeField]
	private bool loadBindingsFromFile = false;
	void Awake(){
		initDefaultKeys();
	}

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

	#region Mouse control
	public void constrainMouseToScreen(bool state) {
		if(state)
		Cursor.lockState = CursorLockMode.Confined;
		else
		Cursor.lockState = CursorLockMode.None;
	}
	#endregion


	#region Dictionary 
	private Dictionary<string, KeyRequirements> keySettings;
	private Dictionary<string, KeyRequirements> dynamicKeys;

	private Dictionary<string, KeypressCallback> keyPressCallbacks;

	private void initDefaultKeys() {
		keySettings = new Dictionary<string, KeyRequirements> {
			{ "mainAction",	new KeyRequirements(	new KeyCode[]	{ KeyCode.E },		KeypressState.buttonDown) },

			{ "moveUp",		new KeyRequirements(	new KeyCode[]	{ KeyCode.W },		KeypressState.buttonHeld) },
			{ "moveDown",	new KeyRequirements(	new KeyCode[]	{ KeyCode.S },		KeypressState.buttonHeld) },
			{ "moveLeft",	new KeyRequirements(	new KeyCode[]	{ KeyCode.A },		KeypressState.buttonHeld) },
			{ "moveRight",	new KeyRequirements(	new KeyCode[]	{ KeyCode.D },		KeypressState.buttonHeld) },

			{ "card1",		new KeyRequirements(	new KeyCode[]	{ KeyCode.Alpha1 },	KeypressState.buttonDown) },
			{ "card2",		new KeyRequirements(	new KeyCode[]	{ KeyCode.Alpha2 },	KeypressState.buttonDown) },
			{ "card3",		new KeyRequirements(	new KeyCode[]	{ KeyCode.Alpha3 },	KeypressState.buttonDown) },
			{ "card4",		new KeyRequirements(	new KeyCode[]	{ KeyCode.Alpha4 },	KeypressState.buttonDown) },
		};

		keyPressCallbacks = new Dictionary<string, KeypressCallback>();
		foreach(KeyValuePair<string, KeyRequirements> item in keySettings) {
			keyPressCallbacks.Add(item.Key, emptyCallback);
		}
	}

	/// <summary>
	/// add a callback to a callbackName
	/// callbackName must already exist
	/// use dynamicKeys for temporary keys
	/// </summary>
	/// <param name="callbackName"></param>
	/// <param name="callback"></param>
	public void setCallback(string callbackName, KeypressCallback callback) {
		KeypressCallback method;
		if(keyPressCallbacks.TryGetValue(callbackName, out method)){
			method += callback;
		} else {
			Debug.LogWarning("set: no key binding found with the name " + callbackName);
		}
	}

	public void removeCallback(string callbackName, KeypressCallback callback) {
		KeypressCallback method;
		if(keyPressCallbacks.TryGetValue(callbackName, out method)) {
			try {
				method -= callback; //WARNING untested
			} catch(System.Exception) {
				Debug.LogWarning("that callback is not found in " + callbackName);
			}
		} else {
			Debug.LogWarning("remove: no key binding found with the name " + callbackName);
		}
	}

	#endregion


	#region setDefaultBindings
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

	#endregion

	#region action delegates
	/*	instructions to assign a method to a KeypressCallback 
		nameOfDelegateContainer += methodName;
		the plus is necessary since multiple methods classes may want to work with the same callback
		*/
	public delegateCallbacks buttonCallbackFunctions;
	/// <summary>
	/// used to prevent reference errors in case no actions are set to a button
	/// </summary>
	public void emptyCallback() {
		
	}

	#endregion

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
	}

	/// <summary>
	/// checks to see if all the keys pressed.
	/// </summary>
	/// <returns><c>true</c>, if all keys pressed was corrected, <c>false</c> otherwise.</returns>
	/// <param name="keys">Keys.</param>
	private bool correctKeysPressed(KeyCode[] keys){
		int firstKeyCode = 1; //key code 0 is extra info
		if (keys == null || keys.Length <=1)
			return false;
		if(keys[firstKeyCode] == default(KeyCode)){ // no key set
			return false;
		}

		if(keys[0] == KeyCode.F12) {
			if(getInputKeyInfo(KeyCode.F10,keys[1]) && getInputKeyInfo(KeyCode.F9, keys[2])) {
				return true; // all keys required are down
			}
		}

		if (getInputKeyInfo(keys[0], keys[1])) {
			return true; // key required is in correct position
		}
		
		return false; 
	}
	#endregion

}

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

[System.Serializable]
public struct KeyRequirements {
	public KeyRequirements(KeyCode[] keyRequirements, KeypressState state){
		mouse = -1;
		keys = keyRequirements;
		pressState = state;
	}

	public KeyRequirements(int mouseButton, KeypressState state) {
		if(mouseButton <0 || mouseButton > 3) {
			Debug.Log("error, not valid mouse button");
		}
		mouse = mouseButton;
		keys = new KeyCode[0];
		pressState = state;
	}

	public int mouse;
	public KeyCode[] keys;
	public KeypressState pressState;
}

[System.Serializable]
public struct TestStruct {
	public KeyRequirements[] kr;
	public int val1;
}

public enum KeypressState : int {
	buttonDown,
	buttonUp,
	buttonHeld
}

public struct delegateCallbacks {
	public KeypressCallback mainAction; //can store multuple methods to call when the button is pressed
	public KeypressCallback moveUp;
	public KeypressCallback moveDown;
	public KeypressCallback moveLeft;
	public KeypressCallback moveRight;
	public KeypressCallback[] activateCard;
}