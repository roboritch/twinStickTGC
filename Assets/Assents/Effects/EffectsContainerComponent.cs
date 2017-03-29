using EffectNS; 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class EffectsContainerComponent : MonoBehaviour {
	private Actor actor;
	//indexed by effect type
	private LinkedList<TriggerdEffect>[] triggeredEffects;
	private LinkedList<TimedAndTriggeredEffect>[] timedAndTriggerdEffects;
	private LinkedList<TimedEffect>[] timedEffects;

	private void initEventStorage() {
		int numberOfBoxes = Enum.GetNames(typeof(EffectTypes)).Length;
		triggeredEffects = new LinkedList<TriggerdEffect>[numberOfBoxes];
		timedAndTriggerdEffects = new LinkedList<TimedAndTriggeredEffect>[numberOfBoxes];
		timedEffects = new LinkedList<TimedEffect>[numberOfBoxes];

		for (int i = 0; i < numberOfBoxes; i++) {
			triggeredEffects[i] = new LinkedList<TriggerdEffect>();
			timedAndTriggerdEffects[i] = new LinkedList<TimedAndTriggeredEffect>();
			timedEffects[i] = new LinkedList<TimedEffect>();
		}	
	}


	public void addEffect(Effect effect) {
		int effectIndex = (int)effect.getEffectType;
		if (effect is TimedAndTriggeredEffect) {
			timedAndTriggerdEffects[effectIndex].AddLast((TimedAndTriggeredEffect)effect);
		} else if (effect is TriggerdEffect) {
			triggeredEffects[effectIndex].AddLast((TriggerdEffect)effect);
		}else if (effect is TimedEffect) {
			timedEffects[effectIndex].AddLast((TimedEffect)effect);
		}else {
			Debug.LogWarning("not a valid effect");
		}
	}

	public void triggerEffects(EffectTypes effectType,Actor actor) {
		LinkedList<TriggerdEffect> listOfEffects = triggeredEffects[(int)effectType]; //list of all effects of that type
		if (!(listOfEffects.Count == 0)) {
			LinkedListNode<TriggerdEffect> curNode = listOfEffects.First;
			while (curNode != null) {
				TriggerdEffect effect = curNode.Value;
				if (effect.NumberOfUsesLeft <= 0) {
					LinkedListNode<TriggerdEffect> tempNode = curNode.Next;
					listOfEffects.Remove(curNode);
					curNode = tempNode;
				} else {
					curNode.Value.applyEffect(actor);
					curNode = curNode.Next;
				}
			}
		}

		LinkedList<TimedAndTriggeredEffect> listOfEffects2 = timedAndTriggerdEffects[(int)effectType]; //list of all effects of that type
		if (listOfEffects2.Count == 0)
			return;
		LinkedListNode<TimedAndTriggeredEffect> curNode2 = listOfEffects2.First;
		while (curNode2 != null) {
			TimedAndTriggeredEffect effect2 = curNode2.Value;
			if (effect2.NumberOfUsesLeft <= 0) {
				LinkedListNode<TimedAndTriggeredEffect> tempNode = curNode2.Next;
				listOfEffects2.Remove(curNode2);
				curNode2 = tempNode;
			} else {
				curNode2.Value.applyEffect(actor);
				curNode2 = curNode2.Next;
			}
		}

		return effectsApplyed;
	}
	
	void Awake() {
		initEventStorage();
	}

	// Use this for initialization
	void Start () {
		actor = GetComponent<Actor>();
	}

	private void updateTimedAndTriggeredEvents(float timeInc_seconds) {
		for (int i = 0; i < timedAndTriggerdEffects.Length; i++) {
			LinkedListNode<TimedAndTriggeredEffect> curNode = timedAndTriggerdEffects[i].First;
			while (curNode != null) {
				TimedAndTriggeredEffect effect = curNode.Value;
				if (effect.NumberOfUsesLeft <= 0) { //check to see if Effect should be removed
					LinkedListNode<TimedAndTriggeredEffect> tempNode = curNode.Next;
					timedAndTriggerdEffects[i].Remove(curNode);
					curNode = tempNode;
				} else {
					if (effect.incrmentTimer(timeInc_seconds)) {//Inc effectTimer
						effect.applyEffect(actor); //apply the effect to the actor
					}
					curNode = curNode.Next;
				}
			}
		}
	}


	private void updateTimedEffects(float timeInc_seconds) {
		for (int i = 0; i < timedEffects.Length; i++) {
			LinkedListNode<TimedEffect> curNode = timedEffects[i].First;
			while (curNode != null) {
				TimedEffect effect = curNode.Value;
				if (effect.NumberOfUsesLeft <= 0) { //check to see if Effect should be removed
					LinkedListNode<TimedEffect> tempNode = curNode.Next;
					timedEffects[i].Remove(curNode);
					curNode = tempNode;
				} else {
					if (effect.incrmentTimer(timeInc_seconds)) {//Inc effectTimer
						effect.applyEffect(actor); //apply the effect to the actor
					}
					curNode = curNode.Next;
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		updateTimedEffects(Time.deltaTime);
		updateTimedAndTriggeredEvents(Time.deltaTime);
	}
}


