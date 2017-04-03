using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageArea : MonoBehaviour {
	private new Collider2D collider;

	private bool countingDown = false;
	/// <summary>
	/// start the coundown till this area damages all actors within it
	/// </summary>
	/// <param name="damageTimeIn_seconds">time in seconds till the area damages all actors in that area</param>
	/// <param name="damage">damage done to any actors in area</param>
	public void startCountDown(float damageTimeIn_seconds) {
		timeTillDamage_seconds = damageTimeIn_seconds;
		countingDown = true;
	}
	/// <summary>
	/// use preset time in prefab 
	/// </summary>
	public void startCountDown() {
		countingDown = true;
	}


	/// <summary>
	/// defaults to instant
	/// </summary>
	[SerializeField]
	private float timeTillDamage_seconds;
	[SerializeField]
	private float timeOffset_seconds = 0f;
	/// <summary>
	/// the amount of damage set in the prefab
	/// </summary>
	public float damageAmount = 0f;
	public DamageTypes damageType;

	public GameObject damageAnimationPrefab;

	private void damageActors() {
		foreach (KeyValuePair<Collider2D,Actor> actor in actorsInArea) {
			actor.Value.takeDamage(damageAmount,damageType);
		}
	}

	private Dictionary<Collider2D, Actor> actorsInArea = new Dictionary<Collider2D, Actor>();

	//add actor to area
	void OnTriggerEnter2D(Collider2D coll) {
		Actor actor = coll.GetComponent<Actor>();
		if(actor != null) {
			actorsInArea.Add(coll, actor);
		}
	}

	//remove actor from area
	void OnTriggerExit2D(Collider2D coll) {
		Actor actor = coll.GetComponent<Actor>();
		if (actor != null) {
			actorsInArea.Remove(coll);
		}
	}

	// Use this for initialization
	void Awake () {
		collider = GetComponent<Collider2D>();
		collider.isTrigger = true;
	}



	void FixedUpdate () {
		if(countingDown)
			if (timeTillDamage_seconds <= 0) {
				damageActors();
				if (damageAnimationPrefab != null) { //play damage animation
					Instantiate(damageAnimationPrefab,transform.position, new Quaternion());
				}
				UnityExtentionMethods.destoryAllChildren(transform);
			} else {
				timeTillDamage_seconds -= Time.fixedDeltaTime;
			}
	}
}
