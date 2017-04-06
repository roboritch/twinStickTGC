using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movment))]
[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Aim))]
public class AI : MonoBehaviour {
	private Actor actor;
	private Actor getActor() {
		return actor;
	}

	private Movment movmentControler;
	private Aim aim;

	private void initAI() {
		actor = GetComponent<Actor>();
		movmentControler = GetComponent<Movment>();
		aim = GetComponent<Aim>();

		if(!manualTarget)
		setTargetByTag("Player");
	}

	#region AI Targeting
	private Actor selectedEnamy;
	public void setTargetByTag(string playerTag) {
		selectedEnamy = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
	}

	private bool manualTarget = false;
	public void setTargetManual(Actor target) {
		selectedEnamy = target;
		manualTarget = true;
	}
	#endregion

	#region strafe code
	[SerializeField]
	private float strafeDistance = 4f;
	/// <summary>
	/// true for left, false for right relative to this actor
	/// </summary>
	private bool strafeDirection = true;
	[SerializeField]
	private float chanceOfStrafeDirectionChange_perSecond = 0.1f;

	/// <summary>
	/// when placed in Update the ai will strafe the selected enamy
	/// </summary>
	private void strafeEnamy() {
		float rng = Random.Range(0, 1);
		if(rng <= chanceOfStrafeDirectionChange_perSecond/Time.deltaTime) {
			strafeDirection = !strafeDirection;
		}

		Vector2 enamyPosition = selectedEnamy.get2dPostion();
		Vector2 AiPosition = actor.get2dPostion();

		float wantedMoveSpeed = movmentControler.getMaxSpeed();
		Vector2 moveVolocity = new Vector2();
		Vector2 wantedLocation = new Vector2();


		//vector from enamy to a perticular distance
		wantedLocation = (AiPosition - enamyPosition).normalized * strafeDistance; 
		wantedLocation += enamyPosition; //vector converted to world space

		moveVolocity = (wantedLocation - AiPosition);
		
		float strafeStartDistance = 1f;
		float distanceFromWantedLocation = strafeStartDistance - (wantedLocation - AiPosition).magnitude ;
		if(distanceFromWantedLocation > 0) {
			Vector2 movePerpendicular = moveVolocity;
			if (strafeDirection) {
				movePerpendicular.x = moveVolocity.y;
			}else {
				movePerpendicular.x = -moveVolocity.y;
			}

			movePerpendicular.y = -moveVolocity.x;
			moveVolocity = moveVolocity.normalized * (strafeStartDistance - distanceFromWantedLocation) + movePerpendicular.normalized * (distanceFromWantedLocation);
		}
		moveVolocity = moveVolocity.normalized * movmentControler.getMaxSpeed();
		movmentControler.setWantedMovment(moveVolocity);

	}
	#endregion


	// Use this for initialization
	void Start () {
		initAI();
	}
	
	// Update is called once per frame
	void Update () {
		if (selectedEnamy != null) { 
			aim.setAim(selectedEnamy.get2dPostion()); //get selectedEnamy location
			aim.lookAtAimLocation();
			strafeEnamy();
		}
	}
}
