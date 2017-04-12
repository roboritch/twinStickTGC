using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movment))]
[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Aim))]
public class AI : MonoBehaviour {
	private Actor actor;
	public Actor getActor() {
		return actor;
	}

	private Movment movementControler;
	public Movment moveControler {
		get {return movementControler;}
	}
	private Aim aim;

	private void initAI() {
		actor = GetComponent<Actor>();
		movementControler = GetComponent<Movment>();
		aim = GetComponent<Aim>();

		if (!manualTarget)
			setTargetByTag("Player");
	}

	#region AI Targeting
	private Actor selectedEnamy;
	/// <summary>
	/// returns self if no enamy selected
	/// </summary>
	/// <returns></returns>
	public Actor getSelectedEnamy() {
		if(selectedEnamy == null) {
			return actor;
		}
		return selectedEnamy;
	}

	public void setTargetByTag(string playerTag) {
		selectedEnamy = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
	}

	private bool manualTarget = false;


	public void setTargetManual(Actor target) {
		selectedEnamy = target;
		manualTarget = true;
	}
	#endregion

	#region Ai Movment Control Setup
	private AIMovementBehaviour currentMoveBehavior;
	[SerializeField]
	private AIMovementType moveType;
	private void changeMovmentType(AIMovementType mType) {
		moveType = mType;

		switch (moveType) {
			case AIMovementType.Still:
				destroyCurrentMoveBehavior(null);
				break;
			case AIMovementType.Strafe:
				destroyCurrentMoveBehavior(typeof(Strafe));
				if(currentMoveBehavior == null) { // add comeponent if not already there
					currentMoveBehavior = transform.GetOrAddComponent<Strafe>();
				}
				break;
			case AIMovementType.Rush:
				break;
			case AIMovementType.GoTo:
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="saveIfThisType">set to be null if no saving is necasry</param>
	private void destroyCurrentMoveBehavior(System.Type saveIfThisType) {
		if(currentMoveBehavior != null) {
			if(currentMoveBehavior.GetType() != saveIfThisType)
				Destroy(currentMoveBehavior);
		}
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
		}
	}
}

public enum AIMovementType {
	Still,
	Strafe,
	Rush,
	GoTo,


}