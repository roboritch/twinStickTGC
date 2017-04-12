using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallExit : MonoBehaviour {

	public void callExit() {
		PauseMenuHandler.Instance.exit();
	}
}
