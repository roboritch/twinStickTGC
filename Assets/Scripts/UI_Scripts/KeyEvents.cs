using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;



public delegate void KeypressCallback();

/// <summary>
/// when added to the game this crates key bindings that can be changed mid game
/// </summary>
public class KeyEvents : Singleton<KeyEvents>{
	
	[SerializeField] private string localFileName = "Generic Bindings";
	[SerializeField] private string localFolderName = "Bindings";
	private ActionBindings currentActionBindings;
	[SerializeField]
	private bool loadBindingsFromFile = false;
	void Start(){
		setDefaultBindings();
		if(!Directory.Exists(appendToLocalPath(localFolderName)))
			Directory.CreateDirectory(appendToLocalPath(localFolderName));
		
		if(!File.Exists(appendToLocalPath(localFolderName + "/" + localFileName))){
			SaveAndLoadXML.saveXML<ActionBindings>(appendToLocalPath(localFolderName + "/" + localFileName), currentActionBindings);
		} else{ //disabled while developing new keys
			if(loadBindingsFromFile)
				tryToLoadBidings();
		}
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


	#region setDefaultBindings
	

	/// <summary>
	/// all default keys must be set here in lew of a key changing screen
	/// </summary>probabilityMultiplyer_4Point
	private void setDefaultBindings(){
		currentActionBindings.mainAction = new KeyCode[] {KeyCode.F9, KeyCode.E};
		buttionCallbackFunctions.mainAction = emptyCallback;

		currentActionBindings.moveUp = new KeyCode[] { KeyCode.F10, KeyCode.W };
		buttionCallbackFunctions.moveUp = emptyCallback;

		currentActionBindings.moveDown = new KeyCode[] { KeyCode.F10, KeyCode.S };
		buttionCallbackFunctions.moveDown = emptyCallback;

		currentActionBindings.moveLeft = new KeyCode[] { KeyCode.F10, KeyCode.A };
		buttionCallbackFunctions.moveLeft = emptyCallback;

		currentActionBindings.moveRight = new KeyCode[] { KeyCode.F10, KeyCode.D };
		buttionCallbackFunctions.moveRight = emptyCallback;

		buttionCallbackFunctions.activateCard = new KeypressCallback[4];

		currentActionBindings.activateCard1 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha1 };
		buttionCallbackFunctions.activateCard[0] = emptyCallback;

		currentActionBindings.activateCard2 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha2 };
		buttionCallbackFunctions.activateCard[1] = emptyCallback;

		currentActionBindings.activateCard3 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha3 };
		buttionCallbackFunctions.activateCard[2] = emptyCallback;

		currentActionBindings.activateCard4 = new KeyCode[] { KeyCode.F9, KeyCode.Alpha4 };
		buttionCallbackFunctions.activateCard[3] = emptyCallback;
	}

	#endregion

	#region action delegates
	/*	instructions to assigen a method to a KeypressCallback 
		nameOfDelegateContainer += methodName;
		the plus is necasary since multuple methods classes may want to work with the same callback
		*/
	public delegateCallbacks buttionCallbackFunctions;
	/// <summary>
	/// used to prevent refrence errors in case no actions are set to a button
	/// </summary>
	public void emptyCallback() {
		
	}

	#endregion

	//key events must be manualy placed in the update
	//can be done with delegets be this is easer to debug
	//with all elements visible
	#region button checks
	void Update(){
		if (correctKeysPressed(currentActionBindings.mainAction)) {
			buttionCallbackFunctions.mainAction();
		}
		if (correctKeysPressed(currentActionBindings.moveUp)) {
			buttionCallbackFunctions.moveUp();
		}
		if (correctKeysPressed(currentActionBindings.moveDown)) {
			buttionCallbackFunctions.moveDown();
		}
		if (correctKeysPressed(currentActionBindings.moveLeft)) {
			buttionCallbackFunctions.moveLeft();
		}
		if (correctKeysPressed(currentActionBindings.moveRight)) {
			buttionCallbackFunctions.moveRight();
		}
		if (correctKeysPressed(currentActionBindings.activateCard1)) {
			buttionCallbackFunctions.activateCard[0]();
		}
		if (correctKeysPressed(currentActionBindings.activateCard2)) {
			buttionCallbackFunctions.activateCard[1]();
		}
		if (correctKeysPressed(currentActionBindings.activateCard3)) {
			buttionCallbackFunctions.activateCard[2]();
		}
		if (correctKeysPressed(currentActionBindings.activateCard4)) {
			buttionCallbackFunctions.activateCard[3]();
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