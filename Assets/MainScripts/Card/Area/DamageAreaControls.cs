using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaControls : MonoBehaviour {
	/// <summary>
	/// this is the damage amount that might effect any damage changes due to effects
	/// </summary>
	[SerializeField]
	private float damageAmountForModifications;
	[SerializeField]
	private DamageTypes damageType;

	private DamageArea[] damageAreas;
	public DamageArea[] getDamageAreas() {
		return damageAreas;
	}

	private Actor damageSorce; 
	public void updateAreasDamageAmounts(Actor damageAreaUser, bool directFromCard) {
		DamageIncreaseData data = damageAreaUser.effects.getDamageIcreaseAmounts(damageAmountForModifications, damageType, directFromCard);
        for (int i = 0; i < damageAreas.Length; i++) {
			data.amount = damageAreas[i].damageAmount;
			damageAreas[i].damageAmount = data.getModifyedDamageAmount();
			damageAreas[i].damageType = damageType;
			damageAreas[i].damageSorce = damageAreaUser.Team;
		}

		damageSorce = damageAreaUser;
	}

	public void startDamageCountdowns(float timeTillDamage_seconds) {
		for (int i = 0; i < damageAreas.Length; i++) {
			damageAreas[i].startCountDown(timeTillDamage_seconds);
		}
	}

	public void startDamageCountdowns() {
		for (int i = 0; i < damageAreas.Length; i++) {
			damageAreas[i].startCountDown();
		}
	}

	#region update location based on aim
	public enum LocationUpdateType {
		None, SetLocation, SetDistanceFromLocation
	}

	[SerializeField]
	private LocationUpdateType locationUpdateType = LocationUpdateType.None;
	[SerializeField]
	private IGetAim aimLocation;
	[SerializeField]
	private Transform startLocation;
	[SerializeField]
	private float staticDistance;

	public void disconectAim() {
		locationUpdateType = LocationUpdateType.None;
	}

	public void setAimLocationCallbacks(Transform aimStartPorint, IGetAim aimCallback, LocationUpdateType type) {
		startLocation = aimStartPorint;
		aimLocation = aimCallback;
		locationUpdateType = type;
		//static distance set in prefab
	}

	/// <summary>
	/// with set area distance
	/// </summary>
	/// <param name="aimStartPorint"></param>
	/// <param name="aimCallback"></param>
	/// <param name="type"></param>
	/// <param name="distance"></param>
	public void setAimLocationCallbacks(Transform aimStartPorint, IGetAim aimCallback, LocationUpdateType type, float distance) {
		setAimLocationCallbacks(aimStartPorint, aimCallback, type);
		staticDistance = distance;
	}

	public void setAreaLocationManual(Vector2 location, float rotationAngle) {
		locationUpdateType = LocationUpdateType.None;
		Vector3 tempLocaiton = location;
		tempLocaiton.z = spriteLayer;
		transform.position = tempLocaiton;
		transform.Rotate(0, 0, rotationAngle);
	}

	private void updateAreaLocationAndRotoation() {
		if(startLocation == null) {
			Debug.Log("actor controlling this area is dead or inactive, destroying area");
			destroyAreas();
			return;
		}

		Vector2 aimLocation;
		Vector3 newLocation;
		Vector3 rot;
        switch (locationUpdateType) {
			case LocationUpdateType.None:
				break;
			case LocationUpdateType.SetLocation:
				this.aimLocation.getAim(out aimLocation);
				newLocation = aimLocation;
				newLocation.z = spriteLayer;
				transform.position = newLocation; // location derectly on aim locaiton
				//updateRotation
				rot = transform.eulerAngles;
				rot.z = startLocation.eulerAngles.z;
				transform.eulerAngles = rot;
				break;
			case LocationUpdateType.SetDistanceFromLocation:
				this.aimLocation.getAim(out aimLocation);
				Vector2 currentActorPosition = startLocation.position; //actor location
				aimLocation = aimLocation - currentActorPosition; // vector from actor to aim
				aimLocation = aimLocation.normalized * staticDistance; //Area damage location to 
				aimLocation += currentActorPosition; // vector from (0,0) to correct area location
				newLocation = aimLocation;
				newLocation.z = spriteLayer;
				transform.position = newLocation;
				//updateRotation
				rot = transform.eulerAngles;
				rot.z = startLocation.eulerAngles.z;
				transform.eulerAngles = rot;
				break;
			default:
				break;
		}
	}

	public void destroyAreas() {
		UnityExtentionMethods.destoryAllChildren(transform);
	}

	[SerializeField]
	private float spriteLayer;

	void Update() {
		updateAreaLocationAndRotoation();
    }
	#endregion


	// Use this for initialization
	void Awake () {
		damageAreas = GetComponentsInChildren<DamageArea>(true);
	}

	void Start() {
		InvokeRepeating("destoryWhenAllareasDone", 2f, 1f);
	}

	private void destoryWhenAllareasDone() {
		if (transform.childCount == 0) {
			Destroy(gameObject);
		}
	}
}
