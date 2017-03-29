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
	private LinkedList<TimedAndTriggeredEffect>[] combEffect;
	private LinkedList<TimedEffect> timedEffects;

	private void initEventStorage() {
		int numberOfBoxes = Enum.GetNames(typeof(EffectTypes)).Length;
		triggeredEffects = new LinkedList<TriggerdEffect>[numberOfBoxes];
		combEffect = new LinkedList<TimedAndTriggeredEffect>[numberOfBoxes];
		for (int i = 0; i < numberOfBoxes; i++) {
			triggeredEffects[i] = new LinkedList<TriggerdEffect>();
			combEffect[i] = new LinkedList<TimedAndTriggeredEffect>();
		}

		timedEffects = new LinkedList<TimedEffect>();
	}


	public void addEffect(Effect effect) {
		
	}


	public delegate returnType EffectInformation<returnType, inputValue>(inputValue value);
	public EffectInformation<r,I> getEffect<r,I>(EffectTypes effectType, I input) {
		EffectInformation<r, I> effectsApplyed = null;

		LinkedList<TriggerdEffect> listOfEffects = triggeredEffects[(int)effectType]; //list of all effects of that type
		if (listOfEffects.Count == 0 )
			return effectsApplyed; // return the null value
		LinkedListNode<TriggerdEffect> curNode = listOfEffects.First;
		while (curNode != null) {
			TriggerdEffect effect = curNode.Value;
			if (effect.NumberOfUsesLeft <= 0) {
				LinkedListNode<TriggerdEffect> tempNode = curNode.Next;
				listOfEffects.Remove(curNode);
				curNode = tempNode;
			} else {
				effectsApplyed += curNode.Value.checkEffectTrigger<r, I>;
				curNode = curNode.Next;
			}
		}

		LinkedList<TimedAndTriggeredEffect> listOfEffects2 = combEffect[(int)effectType]; //list of all effects of that type
		if (listOfEffects2.Count == 0)
			return effectsApplyed; // return prev found values
		LinkedListNode<TimedAndTriggeredEffect> curNode2 = listOfEffects2.First;
		while (curNode2 != null) {
			TimedAndTriggeredEffect effect2 = curNode2.Value;
			if (effect2.NumberOfUsesLeft <= 0) {
				LinkedListNode<TimedAndTriggeredEffect> tempNode = curNode2.Next;
				listOfEffects2.Remove(curNode2);
				curNode2 = tempNode;
			} else {
				effectsApplyed += curNode2.Value.checkEffectTrigger<r, I>;
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
		foreach (var item in combEffect) {
		LinkedListNode<TimedEffect> curNode = item.Value.First;
		while (curNode != null) {
			TimedEffect effect = curNode.Value;
			if (effect.NumberOfUsesLeft <= 0) { //check to see if Effect should be removed
				LinkedListNode<TimedEffect> tempNode = curNode.Next;
				timedEffects.Remove(curNode);
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
		LinkedListNode<TimedEffect> curNode = timedEffects.First;
		while (curNode != null) {
			TimedEffect effect = curNode.Value;
			if(effect.NumberOfUsesLeft <= 0) { //check to see if Effect should be removed
				LinkedListNode<TimedEffect> tempNode = curNode.Next;
				timedEffects.Remove(curNode);
				curNode = tempNode;
			} else {
				if (effect.incrmentTimer(timeInc_seconds)) {//Inc effectTimer
					effect.applyEffect(actor); //apply the effect to the actor
				}
				curNode = curNode.Next;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		updateTimedEffects(Time.deltaTime);
	}
}


