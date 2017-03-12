using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour{

	private Sprite cardArt;

	public float cardTimeReloadTimer_MAX;
	public float cardTimeReloadTimer;

	public float cardResorceCost;

	public abstract void displayDescription(defaultTextHolder decriptionBox);
}
