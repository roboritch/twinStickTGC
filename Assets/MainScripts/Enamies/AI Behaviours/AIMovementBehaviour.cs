using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initAIConnection() must be called by all implmenting classes
/// </summary>
[RequireComponent(typeof(AI))]
public class AIMovementBehaviour : MonoBehaviour {

	protected AI ai;
	/// <summary>
	/// must be called in all child classes start method
	/// Or use Base.Start();
	/// </summary>
	protected void initAIConnection() {
		ai = GetComponent<AI>();
	}

	void Start() {
		initAIConnection();
	}

}
