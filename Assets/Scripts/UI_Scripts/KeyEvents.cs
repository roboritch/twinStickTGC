using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;

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

	#region setDefaultBindings

	/// <summary>
	/// all default keys must be set here in lew of a key changing screen
	/// </summary>
	private void setDefaultBindings(){
		currentActionBindings.mainAction = new KeyCode[] {KeyCode.E};
		currentActionBindings.moveUp = new KeyCode[] { KeyCode.W };
		currentActionBindings.moveDown = new KeyCode[] { KeyCode.S };
		currentActionBindings.moveLeft = new KeyCode[] { KeyCode.A };
		currentActionBindings.moveRight = new KeyCode[] { KeyCode.D };
	}

	#endregion

	#region action delegates
	/*	instructions to assigen a method to a KeypressCallback 
		nameOfDelegateContainer += methodName;
		the plus is necasary since multuple methods classes may want to work with the same callback
		*/

	public delegate void KeypressCallback();
	public KeypressCallback mainAction; //can store multuple methods to call when the button is pressed
	public KeypressCallback moveUp;
	public KeypressCallback moveDown;
	public KeypressCallback moveLeft;
	public KeypressCallback moveRight;
	#endregion

	//key events must be manualy placed in the update
	//can be done with delegets be this is easer to debug
	//with all elements visible
	#region button checks
	void Update(){
		if (correctKeysPressed(currentActionBindings.mainAction)) {
			mainAction();
		}
		if (correctKeysPressed(currentActionBindings.moveUp)) {
			moveUp();
		}
		if (correctKeysPressed(currentActionBindings.moveDown)) {
			moveDown();
		}
		if (correctKeysPressed(currentActionBindings.moveLeft)) {
			moveLeft();
		}
		if (correctKeysPressed(currentActionBindings.moveRight)) {
			moveRight();
		}
	}


	/// <summary>
	/// checks to see if all the keys pressed.
	/// </summary>
	/// <returns><c>true</c>, if all keys pressed was corrected, <c>false</c> otherwise.</returns>
	/// <param name="keys">Keys.</param>
	private bool correctKeysPressed(KeyCode[] keys){
		if(keys == null)
			return false;
		bool firstKeyCanBeHeld = false; //used for midifyers ctrl,shift ext (only supports one at a time)
		if(keys[0] == default(KeyCode)){ // no key set
			return false;
		} else if(keys[0] == KeyCode.LeftControl || keys[0] == KeyCode.RightControl || keys[0] == KeyCode.LeftAlt || keys[0] == KeyCode.RightAlt || keys[0] == KeyCode.LeftShift || keys[0] == KeyCode.RightShift){
			firstKeyCanBeHeld = true;
		}

		foreach( KeyCode key in keys ){
			if(!Input.GetKeyDown(key)){
				if(!firstKeyCanBeHeld){
					return false;
				} else if(!Input.GetKey(key)){
					return false;
				}
			} else if(key == default(KeyCode)){
				return true;
			}
			firstKeyCanBeHeld = false;
		}
		return true;

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
}

