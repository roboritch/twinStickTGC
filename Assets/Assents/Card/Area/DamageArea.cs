using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageArea : MonoBehaviour {
	private new Collider2D collider;

	private bool countingDown = true;
	/// <summary>
	/// start the coundown till this area damages all actors within it
	/// </summary>
	/// <param name="damageTimeIn_seconds">time in seconds till the area damages all actors in that area</param>
	/// <param name="damage">damage done to any actors in area</param>
	public void startCountDown(float damageTimeIn_seconds) {
		timeTillDamage_seconds = damageTimeIn_seconds;
		countingDown = true;
		locationUpdateType = LocationUpdateType.None;
	}

	/// <summary>
	/// defaults to instant
	/// </summary>
	private float timeTillDamage_seconds = 0f;
	public float damageAmount = 0f;


	public GameObject damageAnimationPrefab;

	private void damageActors() {
		foreach (KeyValuePair<Collider2D,Actor> actor in actorsInArea) {
			actor.Value.takeDamage(damageAmount);
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

	#region update location based on aim
	private enum LocationUpdateType {
		None,GivenLocation,DistanceFromLocation
	}

	private LocationUpdateType locationUpdateType = LocationUpdateType.None;
	private IGetAim aimLocation;
	private Transform startLocation;
	private float distanceFromStartLocation;

	[SerializeField]
	private float spriteLayer;


	void Update() {
		if(locationUpdateType == LocationUpdateType.GivenLocation) {
			Vector2 tempLoc;
			aimLocation.getAim(out tempLoc);
			Vector3 newLocation = tempLoc;
			newLocation.z = spriteLayer;
			transform.position = newLocation;
		} else if(locationUpdateType == LocationUpdateType.GivenLocation) {

		}
	}
	#endregion

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
