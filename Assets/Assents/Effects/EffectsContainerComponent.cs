using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class EffectsContainerComponent : MonoBehaviour {
	private Actor actor;
	//indexed by effect type
	private LinkedList<Effect>[] effects;
	private LinkedList<TimedEffect>[] timedAndTriggerdEffects;
	private LinkedList<TimedEffect>[] timedEffects;

	private void initEventStorage() {
		int numberOfBoxes = Enum.GetNames(typeof(EffectTypes)).Length;
		effects = new LinkedList<Effect>[numberOfBoxes];
		timedAndTriggerdEffects = new LinkedList<TimedEffect>[numberOfBoxes];
		timedEffects = new LinkedList<TimedEffect>[numberOfBoxes];

		for (int i = 0; i < numberOfBoxes; i++) {
			effects[i] = new LinkedList<Effect>();
			timedAndTriggerdEffects[i] = new LinkedList<TimedEffect>();
			timedEffects[i] = new LinkedList<TimedEffect>();
		}	
	}

	/// <summary>
	/// if an effect type must be a child of a perticular class
	/// this method insurese there are no errors
	/// </summary>
	/// <param name="effect"></param>
	/// <param name="et"></param>
	/// <returns>true if effect passes sanity check</returns>
	private bool varifyEffectType(Effect effect, EffectTypes et) {
		switch (et) {
			case EffectTypes.damageDealtChange:
				return effect is DamageDealtChange;
			case EffectTypes.damageReceavedChange:
				break;
			case EffectTypes.damageOverTime:
				break;
			case EffectTypes.preventCardPlaying:
				break;
			case EffectTypes.modifyProjectileSpeed:
				return effect is ProjectileSpeedModification;
			case EffectTypes.changeEquipment:
				break;
			case EffectTypes.changeSpeed:
				break;
			default:
				break;
		}
		return true;
	}

	public void addEffect(Effect effect) {
		varifyEffectType(effect, effect.getEffectType);
		int effectIndex = (int)effect.getEffectType;
		if (effect.canBeTriggered) {
			effects[effectIndex].AddLast(effect);
		} else {
			Debug.LogWarning("not a valid effect");
		}
	}

	public void addEffect(TimedEffect effect) {
		varifyEffectType(effect, effect.getEffectType);
		int effectIndex = (int)effect.getEffectType;
		if (effect is TimedEffect && effect.canBeTriggered) {
			timedAndTriggerdEffects[effectIndex].AddLast(effect);
		} else {
			timedEffects[effectIndex].AddLast(effect);
		}
	}

	/// <summary>
	/// gets a trimed array of all valid effects 
	/// </summary>
	/// <param name="effectType"></param>
	/// <returns></returns>
	public Effect[] getListOfEffectsOfType(EffectTypes effectType) {

		LinkedList<TimedEffect> listOfTimedEffects = timedAndTriggerdEffects[(int)effectType]; //list of all effects of that type
		LinkedList<Effect> listOfEffects = effects[(int)effectType]; //list of all effects of that type

		int totalEffectCount = listOfTimedEffects.Count + listOfEffects.Count;
		Effect[] effectGrouping = new Effect[listOfTimedEffects.Count + listOfEffects.Count];
		

		if (!(listOfEffects.Count == 0)) {
			LinkedListNode<Effect> curNode = listOfEffects.First;
			while (curNode != null) {
				Effect effect = curNode.Value;
				if (effect.NumberOfUsesLeft <= 0) {
					LinkedListNode<Effect> tempNode = curNode.Next;
					listOfEffects.Remove(curNode);
					curNode = tempNode;
					totalEffectCount--;
				} else {
					curNode.Value.applyEffect(actor);
					curNode = curNode.Next;
				}
			}
		}

		
		if (listOfTimedEffects.Count == 0) {
			Array.Resize(ref effectGrouping, totalEffectCount);
			return effectGrouping;
		}
			
		LinkedListNode<TimedEffect> curNode2 = listOfTimedEffects.First;
		while (curNode2 != null) {
			TimedEffect effect2 = curNode2.Value;
			if (effect2.NumberOfUsesLeft <= 0) {
				LinkedListNode<TimedEffect> tempNode = curNode2.Next;
				listOfTimedEffects.Remove(curNode2);
				curNode2 = tempNode;
				totalEffectCount--;
			} else {
				curNode2.Value.applyEffect(actor);
				curNode2 = curNode2.Next;
			}
		}

		Array.Resize(ref effectGrouping, totalEffectCount);
		return effectGrouping;
	}

	public void triggerEffects(EffectTypes effectType,Actor actor) {
		Effect[] list = getListOfEffectsOfType(effectType);
		for (int i = 0; i < list.Length; i++) {
			list[i].applyEffect(actor);
		}
	}

	#region Specialized Effect Return Values
	public float getNewProjectileSpeed(float initalProjectileSpeed) {
		ProjectileSpeedModification[] projMods = Array.ConvertAll(getListOfEffectsOfType(EffectTypes.modifyProjectileSpeed), item => (ProjectileSpeedModification)item);
		float speedMultiplyer = 1f;
		for (int i = 0; i < projMods.Length; i++) {
			speedMultiplyer += projMods[i].getProjectileSpeedIncreseMultiplyer(initalProjectileSpeed);
		}
		return initalProjectileSpeed * speedMultiplyer;
	}


	#region Damage Increses
	private DamageIncreaseData updateDamageIncreseData(DamageIncreaseData data) {
		DamageDealtChange[] listOfEffects = Array.ConvertAll(getListOfEffectsOfType(EffectTypes.damageDealtChange), item => (DamageDealtChange)item);
		for (int i = 0; i < listOfEffects.Length; i++) {
			data = listOfEffects[i].damageChanges(data);
			listOfEffects[i].damageChangeEffectUsed();
		}
		return data;
	}

	/// <summary>
	/// increases a damage amount by some value based on the Effects on this actor
	/// this should be called whenever the damage of an attack is assigned (Prejectile created
	/// area damage removed from player control), some modifyers can be changed
	/// while the player has an item equiped
	/// </summary>
	/// <param name="amount">the base amount of damage to be dealt</param>
	/// <param name="damageType">the type of damage being dealt</param>
	/// <param name="fromCard">is the damage from a card or from equitment
	/// used if one card increses the damge of another</param>
	/// <returns></returns>
	public float modifyDamage(float amount, DamageTypes damageType, bool fromCard) {
		DamageIncreaseData damageIncreaseData = new DamageIncreaseData(amount, damageType, fromCard);
		damageIncreaseData = updateDamageIncreseData(damageIncreaseData);
        return damageIncreaseData.getModifyedDamageAmount();
	}

	public DamageIncreaseData getDamageIcreaseAmounts(float amount, DamageTypes damageType, bool fromCard) {
		DamageIncreaseData damageIncreaseData = new DamageIncreaseData(amount, damageType, fromCard);
		damageIncreaseData = updateDamageIncreseData(damageIncreaseData);
		return damageIncreaseData;
    }
	#endregion
		

	#endregion



	void Awake() {
		initEventStorage();
	}

	// Use this for initialization
	void Start () {
		actor = GetComponent<Actor>();
	}

	private void updateTimedAndTriggeredEvents(float timeInc_seconds) {
		for (int i = 0; i < timedAndTriggerdEffects.Length; i++) {
			LinkedListNode<TimedEffect> curNode = timedAndTriggerdEffects[i].First;
			while (curNode != null) {
				TimedEffect effect = curNode.Value;
				if (effect.NumberOfUsesLeft <= 0) { //check to see if Effect should be removed
					LinkedListNode<TimedEffect> tempNode = curNode.Next;
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


