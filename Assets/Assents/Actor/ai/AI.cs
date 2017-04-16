using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movment))]
[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Aim))]
public class AI : MonoBehaviour {

	#region AI init and core variables
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

		setTeam();
		getDeckComponent();
	}

	#endregion

	#region AI Targeting
	private Actor selectedEnemy;
	/// <summary>
	/// returns self if no enemy selected
	/// </summary>
	/// <returns></returns>
	public Actor getSelectedEnemy() {
		if(selectedEnemy == null) {
			return actor;
		}
		return selectedEnemy;
	}

	public void setTargetByTag(string playerTag) {
		selectedEnemy = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
	}

	private bool manualTarget = false;


	public void setTargetManual(Actor target) {
		selectedEnemy = target;
		manualTarget = true;
	}
	#endregion

	#region AI Movement Control Setup
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
				if(currentMoveBehavior == null) { // add component if not already there
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
	/// <param name="saveIfThisType">set to be null if no saving is necessary</param>
	private void destroyCurrentMoveBehavior(System.Type saveIfThisType) {
		if(currentMoveBehavior != null) {
			if(currentMoveBehavior.GetType() != saveIfThisType)
				Destroy(currentMoveBehavior);
		}
	}
	#endregion

	#region Simple AI Deck and Card Control
	private Deck deck;
	private void getDeckComponent() {
		deck = GetComponent<Deck>();
	}

	/// <summary>
	/// draw and immediately use a card from the deck
	/// </summary>
	private void useCardInDeck() {
		Card card;
		if(CardConstructor.constructCard(deck.drawCard(), out card)) {
			card.useCard(actor);
		}
	}

	[SerializeField]
	private AICardUseBehavior cardUse = AICardUseBehavior.none;
	[SerializeField]
	private float delayOrInterval_seconds = 1f;

	private void cardUseBehavior() {
		switch (cardUse) {
			case AICardUseBehavior.none:
				break;
			case AICardUseBehavior.onStart_1Card:
				useCardInDeck();
                break;
			case AICardUseBehavior.onStart_2Cards:
				useCardInDeck();
				useCardInDeck();
                break;
			case AICardUseBehavior.delay:
				Invoke("useCardInDeck", delayOrInterval_seconds);
				break;
			case AICardUseBehavior.interval:
				InvokeRepeating("useCardInDeck", delayOrInterval_seconds, delayOrInterval_seconds);
				break;
			case AICardUseBehavior.custom:
				//TODO add custom card execution behaviors with a new script
				break;
			default:
				break;
		}
	}

	#endregion

	#region AI team
	[SerializeField]
	private DamageSources AITeam = DamageSources.AIGroup1;
	private void setTeam() {
		//actor.Team = AITeam;
	}

	#endregion

	// Use this for initialization
	void Start () {
		initAI();
		cardUseBehavior();
    }
	
	// Update is called once per frame
	void Update () {
		if (selectedEnemy != null) { 
			aim.setAim(selectedEnemy.get2dPostion()); //get selectedEnamy location
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

//behavior 
public enum AICardUseBehavior {
	none,
	onStart_1Card,
	onStart_2Cards,
	delay,
	interval,
	custom
}