using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment {

	protected CardLocation possibleCardLocations;
	public CardLocation getPossibleCardLocations {
		get {
			return possibleCardLocations;
		}
	}

	public CardLocation inCardSlots;


	public abstract void initalizeEquipment(Actor actor);
	public abstract void destroyEquipment();


}
