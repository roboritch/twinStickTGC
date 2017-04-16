using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafe : AIMovementBehaviour {

	#region strafe code
	[SerializeField]
	public float strafeDistance = 2f;
	/// <summary>
	/// true for left, false for right relative to this actor
	/// </summary>
	private bool strafeDirection = true;
	/// <summary>
	/// set to 0 if consistant strafe direction is wanted
	/// </summary>
	[SerializeField]
	public float chanceOfStrafeDirectionChange_perSecond = 0.1f;

	/// <summary>
	/// when placed in Update the ai will strafe the selected enamy
	/// </summary>
	private void strafeEnamy(Vector2 enamyPosition, Vector2 AiPosition) {
		float rng = Random.Range(0.0001f, 1);
		if (rng <= chanceOfStrafeDirectionChange_perSecond / Time.deltaTime) {
			strafeDirection = !strafeDirection;
		}

		float wantedMoveSpeed = ai.moveControler.getMaxSpeed();
		Vector2 moveVolocity = new Vector2();
		Vector2 wantedLocation = new Vector2();


		//vector from enamy to a perticular distance
		wantedLocation = (AiPosition - enamyPosition).normalized * strafeDistance;
		wantedLocation += enamyPosition; //vector converted to world space

		moveVolocity = (wantedLocation - AiPosition);

		float strafeStartDistance = 1f;
		float distanceFromWantedLocation = strafeStartDistance - (wantedLocation - AiPosition).magnitude;
		if (distanceFromWantedLocation > 0) {
			Vector2 movePerpendicular = moveVolocity;
			if (strafeDirection) {
				movePerpendicular.x = moveVolocity.y;
			} else {
				movePerpendicular.x = -moveVolocity.y;
			}

			movePerpendicular.y = -moveVolocity.x;
			moveVolocity = moveVolocity.normalized * (strafeStartDistance - distanceFromWantedLocation) + movePerpendicular.normalized * (distanceFromWantedLocation);
		}
		moveVolocity = moveVolocity.normalized * wantedMoveSpeed;
		ai.moveControler.setWantedMovment(moveVolocity);

	}
	#endregion


	// Use this for initialization
	void Start () {
		initAIConnection();
	}
	
	// Update is called once per frame
	void Update () {
		strafeEnamy(ai.getSelectedEnemy().get2dPostion(), ai.getActor().get2dPostion());
	}
}
