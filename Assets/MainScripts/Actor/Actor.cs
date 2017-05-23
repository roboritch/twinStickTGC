using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Aim))]
[RequireComponent(typeof(EffectsContainerComponent))]
public class Actor : MonoBehaviour , IDamageable {

	#region modifier delegates	
	public delegate float floatModifyer(float input);
	#endregion

	#region health
	private bool dead = false;
	/// <summary>
	/// this method must be checked by any script that saves
	/// references an actor
	/// </summary>
	/// <returns></returns>
	public bool isDead() {
		return dead;
	}

	[SerializeField] private float health_MAX = 100;
	[SerializeField] private float health_MIN = 0; //some cards may set this > 0 for a short time
	[SerializeField] private float health;
	[SerializeField] private float invincibilityFrames_seconds = 2f;
	private HealthBarControler healthUI;
	private void initHealth() {
		health = health_MAX;
		if (playerControled) {
			GameObject healthDisplay = GameObject.FindGameObjectWithTag("HeathUI");
			if (healthDisplay != null) {
				healthUI = healthDisplay.GetComponent<HealthBarControler>();
				healthUI.updateHealthDispley(health_MAX, health);
            }
		}
	}

	#region IDamagable
	/// <summary>
	/// apply the specified damage to this Actor
	/// </summary>
	/// <param name="amount">amount of damage</param>
	/// <returns>true if damage is taken</returns>
	public bool takeDamage(float amount, DamageTypes damageType,DamageSources damageSorce) {
		if(ignoreDamage(damageSorce, damageType)) {
			return false;
		}
		health -= amount;
		if(health < health_MIN) {
			health = health_MIN;
		}
		if (health <= 0) {
			actorDies();
		}
		takeDamageGrapic_flashColor(.8f);
		if (healthUI != null) {
			healthUI.updateHealthDispley(health_MAX, health);
		}
		return true;
	}

	private void actorDies() {
		UnityExtentionMethods.destoryAllChildren(transform);
		dead = true;
		//TAG: level analytics
		if(team != DamageSources.player1) {
			LevelAnalytics.Instance.enemyDestroyed();
		}
	}

	public bool blocksDamage(float amount, DamageTypes damageType) {
		//TODO check for damage blocking effects
		return false;
	}

	//ignore 
	[SerializeField]
	private DamageSources team = DamageSources.none;
	public DamageSources Team {
		get { return team; }
		set { team = value; }
	}

	/// <summary>
	/// prevents teammatesFrom blocking attacks
	/// </summary>
	/// <param name="damageSorce"></param>
	/// <param name="damageType"></param>
	/// <returns></returns>
	public bool ignoreDamage(DamageSources damageSorce, DamageTypes damageType) {
		if((team & damageSorce) == damageSorce) {
			return true;
		}
		return false;
	}
	#endregion

	public new Collider2D collider;
	#endregion
	
	#region Damage Graphics 
	private void initGrapics() {
		image = GetComponent<SpriteRenderer>();
		color_Default = image.color;
		color_Damaged = Color.red;
	}

	private Color color_Damaged;
	private Color color_Default;
	private SpriteRenderer image;

	private float flashLength = 0;
	private void updateSpriteDamage() {
		if (flashLength <= 0) {
			setColor_Default();
			return;
		}
		flashLength -= Time.deltaTime;
	}

	private void takeDamageGrapic_flashColor(float flashlength_seconds) {
		setColor_Damaged();
		flashLength = flashlength_seconds;
	}

	#region flashing in frames
	private void takeDamageGrapic_invFrames(float flashDeration_seconds) {
		InvokeRepeating("setColor_Damaged", 0, .3f);
		InvokeRepeating("setColor_Default", .15f, .3f);
		Invoke("endDamageGrapic", flashDeration_seconds);
	}

	private void endDamgeGrapic() {
		CancelInvoke("setColor_Damaged");
		CancelInvoke("setColor_Default");
		setColor_Default();
	}
	#endregion

	private void setColor_Damaged() {
		image.color = color_Damaged;
	}

	private void setColor_Default() {
		image.color = color_Default; 
	}



	#endregion

	#region Equipment
	private Equipment equipSlot_Above;
	private Equipment equipSlot_Right;
	private Equipment equipSlot_Below;
	private Equipment equipSlot_Left;
	private Equipment equipSlot_OnTopOf;

	///used by equipment that does not use a slot
	///but is still connected to the actor somehow
	private List<Equipment> equipSlot_Global;

	private void initEquipment() {
		updateEquipPoints();
	}


	private bool checkIfTakesUpSlot(Equipment equipment, CardLocation slotType) {
		return (equipment.getPossibleCardLocations & slotType) == slotType;
	}

	private bool trySetEquipSlot(Equipment newEquipment,CardLocation location, ref Equipment equipSlot, bool overrideCurrent) {
		//check if this slot is used by the newEquipment
		if(checkIfTakesUpSlot(equipSlot, location)) {
			//check to see if slot is already in use
			if(!overrideCurrent) {
				if(equipSlot != null) {
					return false;
				}
			}
			//set the new equipment
			if(equipSlot != null) {
				equipSlot.destroyEquipment();
			}
			equipSlot = newEquipment;
			return true;
		}
		return false;
	}

	public bool addEquipment(Equipment equipment,bool overrideCurrent) {
		if(checkIfTakesUpSlot(equipment, CardLocation.noSlot)) {
			equipSlot_Global.Add(equipment);
			//if this slot is used no other slot should be used
			return true;
		}

		bool setAll = (equipment.getPossibleCardLocations & CardLocation.all) == CardLocation.all;
		bool someIsSet = false;

		//TODO change part of CardLocation to inalterable enum
		if(trySetEquipSlot(equipment, CardLocation.above, ref equipSlot_Above, overrideCurrent)) {
			if(!setAll) {
				return true;
			}
			someIsSet = true;
		}
		if(trySetEquipSlot(equipment, CardLocation.right, ref equipSlot_Right, overrideCurrent)) {
			if(!setAll) {
				return true;
			}
			someIsSet = true;
		}
		if(trySetEquipSlot(equipment, CardLocation.below, ref equipSlot_Below, overrideCurrent)) {
			if(!setAll) {
				return true;
			}
			someIsSet = true;
		}
		if(trySetEquipSlot(equipment, CardLocation.left, ref equipSlot_Left, overrideCurrent)) {
			if(!setAll) {
				return true;
			}
			someIsSet = true;
		}
		if(trySetEquipSlot(equipment, CardLocation.onTopOf, ref equipSlot_OnTopOf, overrideCurrent)) {
			if(!setAll) {
				return true;
			}
			someIsSet = true;
		}
		
		return someIsSet;
	}

	//4 points on the actor that objects can be placed
	#region equip points
	private Vector3 equipPoint_Above;
	private Vector3 equipPoint_Right;
	private Vector3 equipPoint_Below;
	private Vector3 equipPoint_Left;
	private Vector3 equipPoint_OnTopOf;

	/// <summary>
	/// must be called on initialization
	/// 
	/// this must be called whenever an actors colder changes
	/// (including when an actor is scaled)
	/// </summary>
	private void updateEquipPoints() {
		Bounds bounds = collider.bounds;
		Vector3 centerPoint = bounds.center;
		float offset = 0.1f;

		equipPoint_Above = transform.InverseTransformPoint(bounds.ClosestPoint(centerPoint + new Vector3(0,20f)) + new Vector3(0, offset));
		equipPoint_Right = transform.InverseTransformPoint(bounds.ClosestPoint(centerPoint + new Vector3(20f,0)) + new Vector3(offset,0));
		equipPoint_Below = transform.InverseTransformPoint(bounds.ClosestPoint(centerPoint + new Vector3(0,-20f)) + new Vector3(0, -offset));
		equipPoint_Left = transform.InverseTransformPoint(bounds.ClosestPoint(centerPoint + new Vector3(-20f,0)) + new Vector3(-offset, 0));

		equipPoint_OnTopOf = transform.InverseTransformPoint(centerPoint);
	}

	/// <summary>
	/// get the location of an equip point in the local space of this actor
	/// </summary>
	/// <param name="pointLocation"></param>
	/// <returns></returns>
	public Vector3 getEquipPoint(CardLocation pointLocation) {
		switch(pointLocation) {
			case CardLocation.instant:
			break;
			case CardLocation.persistant:
			break;
			case CardLocation.above:
			return equipPoint_Above;
			case CardLocation.right:
			return equipPoint_Right;
			case CardLocation.below:
			return equipPoint_Below;
			case CardLocation.left:
			return equipPoint_Left;
			case CardLocation.onTopOf:
			return equipPoint_OnTopOf;
			case CardLocation.noSlot:
			break;
			case CardLocation.all:
			break;
			default:
			break;
		}

		Debug.LogError("no valid location found");
		return new Vector3();
	}




	#endregion




	#endregion

	//all connections to KeyEvents should be here and be disable
	#region button callbacks
	[SerializeField] private bool playerControled = false;
	private void initButtonCallbacks() {
		if (playerControled) { 
			KeyEvents.Instance.setCallback("mainAction", mainActionPressed);
		}
	}

	/// <summary>
	/// called when main button is pressed (set in KeyEvents)
	/// </summary>
	public void mainActionPressed() {
		
	}
	#endregion

	#region Effects
	public EffectsContainerComponent effects;
	private void initEffects() {
		effects = GetComponent<EffectsContainerComponent>();
	}
	
	public bool addEffect(Effect effect) {
		effects.addEffect(effect);
		return true;
	}
	#endregion

	#region Hand of Cards

	#endregion

	#region MonoBehaviour methods
	void Awake() {
		collider = GetComponent<Collider2D>();
		initGrapics();
		initEffects();
		aimObject = GetComponent(typeof(IGetAim)) as IGetAim;
		initEquipment();
	}

	// Use this for initialization
	void Start () {
		initHealth();
		initButtonCallbacks();
	}
	
	// Update is called once per frame
	void Update () {
		updateSpriteDamage();
	}
	#endregion

	#region card helper functions
	public Vector2 get2dPostion() {
		return new Vector2(transform.position.x, transform.position.y);
	}
	
	/// <summary>
	/// aim of this object
	/// </summary>
	private IGetAim aimObject;
	public IGetAim getActorAimCallback() {
		return aimObject;
	}

	public Vector2 getNormalizedAim(Vector2 startPoint) {
		if(aimObject == null) {
			Debug.LogWarning("No aim in object!\n" + transform.GetInstanceID());
			return new Vector2();
		}
		Vector2 aimLocation;
		aimObject.getAim(out aimLocation);
		aimLocation = aimLocation - startPoint; //vector from actor to aim
		aimLocation = aimLocation.normalized;
		return aimLocation;
	}

	public Vector2 getAimLocation() {
		Vector2 aim;
		aimObject.getAim(out aim);
		return aim;
	}
	#endregion


}
