using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageArea : MonoBehaviour {
	private new Collider2D collider;

	private bool countingDown = false;
	/// <summary>
	/// start the countdown till this area damages all actors within it
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
	private float timeTillDamagePointInDamageAnimation_seconds;
	[SerializeField]
	private float timeOffset_seconds = 0f;
	/// <summary>
	/// the amount of damage set in the prefab
	/// or the amount set by the controls
	/// </summary>
	public float damageAmount = 0f;
	public DamageTypes damageType;
	public DamageSources damageSorce;

	public GameObject damageAnimationPrefab;


	#region IDamageable finder and damage application 
	private void damageActors() {
		LinkedList<IDamageable> damageableObjectList = new LinkedList<IDamageable>();
		//can't change values while iterating over dictionary (IDamageable ref changed if object is destroyed)
		foreach(IDamageable damagableObject in actorsInArea.Values) {
			damageableObjectList.AddLast(damagableObject);
		}

		foreach(IDamageable damagableObject in damageableObjectList) {
			damagableObject.takeDamage(damageAmount, damageType, damageSorce);
		}

	}

	/// <summary>
	/// list of all colliders currently in the area
	/// </summary>
	private Dictionary<int, IDamageable> actorsInArea = new Dictionary<int, IDamageable>();
	
	//add IDamageable object to area
	void OnTriggerEnter2D(Collider2D coll) {
		IDamageable actor = coll.GetComponent<IDamageable>();
		if(actor != null) {
			if(!actorsInArea.ContainsKey(coll.GetInstanceID()))
				actorsInArea.Add(coll.GetInstanceID(), actor);
		}
	}

	//remove IDamageable object from area list
	void OnTriggerExit2D(Collider2D coll) {
		IDamageable actor = coll.GetComponent<IDamageable>();
		if(actor != null) {
			actorsInArea.Remove(coll.GetInstanceID());
		}
	}
	#endregion



	// Use this for initialization
	void Awake () {
		collider = GetComponent<Collider2D>();
		collider.isTrigger = true;
	}


	private bool animationRuning = false;
	void FixedUpdate () {
		if (countingDown) {
			if (timeTillDamage_seconds <= 0) {
				if (damageAnimationPrefab != null) { //play damage animation
					Instantiate(damageAnimationPrefab, transform.position, new Quaternion());
					countingDown = false;
					animationRuning = true;
					SpriteRenderer sr = GetComponent<SpriteRenderer>();
					if(sr != null) {
						sr.sprite = null;
					}
				}
			} else {
				timeTillDamage_seconds -= Time.fixedDeltaTime;
			}
		}else if (animationRuning) {
			timeTillDamagePointInDamageAnimation_seconds -= Time.fixedDeltaTime;
			if (timeTillDamagePointInDamageAnimation_seconds <= 0) {
				damageActors();
				UnityExtentionMethods.destoryAllChildren(transform);
			}
		}
	}
}
