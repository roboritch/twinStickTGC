using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : AIMovementBehaviour {


	private void moveToEnamy(Vector2 enamyPosition, Vector2 AiPosition) {
		float wantedMoveSpeed = ai.moveControler.getMaxSpeed();
		Vector2 moveVolocity = enamyPosition - AiPosition;

		moveVolocity = moveVolocity.normalized * wantedMoveSpeed;
		ai.moveControler.setWantedMovment(moveVolocity);
	}
		
	// Use this for initialization
	void Start () {
		initAIConnection();
	}
	
	// Update is called once per frame
	void Update () {
		moveToEnamy(ai.getSelectedEnamy().get2dPostion(), ai.getActor().get2dPostion());
	}
}
