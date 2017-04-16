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
	public DamageSources team;

	public GameObject damageAnimationPrefab;

	private void damageActors() {
		foreach (KeyValuePair<Collider2D, IDamageable> actor in actorsInArea) {
			if(actor.Value != null)
				actor.Value.takeDamage(damageAmount,damageType,team);
		}
	}

	private Dictionary<Collider2D, IDamageable> actorsInArea = new Dictionary<Collider2D, IDamageable>();

	//add actor to area
	void OnTriggerEnter2D(Collider2D coll) {
		IDamageable actor = coll.GetComponent<IDamageable>();
		if(actor != null) {
			if(!actorsInArea.ContainsKey(coll))
				actorsInArea.Add(coll, actor);
		}
	}

	//remove actor from area
	void OnTriggerExit2D(Collider2D coll) {
		IDamageable actor = coll.GetComponent<IDamageable>();
		if (actor != null) {
			actorsInArea.Remove(coll);
		}
	}

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
