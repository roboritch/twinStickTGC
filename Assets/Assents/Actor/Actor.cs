using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Aim))]
[RequireComponent(typeof(EffectsContainerComponent))]
public class Actor : MonoBehaviour , IDamageable {

	#region modifyer delegates	
	public delegate float floatModifyer(float input);
	#endregion

	#region health
	[SerializeField] private float health_MAX = 100;
	[SerializeField] private float health_MIN = 0; //some cards may set this > 0 for a short time
	[SerializeField] private float health;
	[SerializeField] private float invincibilityFrames_seconds = 2f;

	private void healthInit() {
		health = health_MAX;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="amount">amount of damage</param>
	/// <returns>true if damage is taken</returns>
	public bool takeDamage(float amount) {
		if (IsInvoking("setColor_Damaged") || IsInvoking("setColor_Default")) {
			return false;
		}
		health -= amount;
		takeDamageGrapic_flashColor(.8f);
		return true;
	}
	

	public new Collider2D collider;
	#endregion

	private void initGrapics() {
		image = GetComponent<SpriteRenderer>();
		color_Default = image.color;
		color_Damaged = Color.red;
	}

	#region Damage Grapics 
	private Color color_Damaged;
	private Color color_Default;
	private SpriteRenderer image;

	private void takeDamageGrapic_flashColor(float flashlength_seconds) {
		setColor_Damaged();
		Invoke("setColor_Default", flashlength_seconds);
	}


	private void takeDamageGrapic_invFrames(float flashDeration_seconds) {
		InvokeRepeating("setColor_Damaged", 0, .3f);
		InvokeRepeating("setColor_Default", .15f, .3f);
		Invoke("endDamageGrapic", flashDeration_seconds);
	}

	private void setColor_Damaged() {
		image.color = color_Damaged;
	}

	private void setColor_Default() {
		image.color = color_Default; 
	}

	private void endDamgeGrapic() {
		CancelInvoke("setColor_Damaged");
		CancelInvoke("setColor_Default");
		setColor_Default();
	}

	#endregion

	//all conections to KeyEvents shoud be here and be disablable
	#region button callbacks
	[SerializeField] private bool playerControled = true;
	private void initButtonCallbacks() {
		if (playerControled) { 
			KeyEvents.Instance.buttionCallbackFunctions.mainAction += mainActionPressed;
		}
	}

	/// <summary>
	/// called when main button is pressed (set in KeyEvents)
	/// </summary>
	public void mainActionPressed() {
		
	}
	#endregion

	#region Effects
	EffectsContainerComponent effects;
	private void initEffects() {
		effects = GetComponent<EffectsContainerComponent>();
	}


	private DamageIncreaseData damageIncreasData_EffectTemp;
	public float damageAddition_EffectTemp;
	public float damageMultiplication_EffectTemp;
	public bool damage0Flag_EffectTemp;

	public DamageIncreaseData getDamageIncreseContainer() {
		return damageIncreasData_EffectTemp;
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
	public float modifyDamage(float amount,DamageTypes damageType, bool fromCard) {
		damage0Flag_EffectTemp = false;
		damageMultiplication_EffectTemp = 1f;
		damageAddition_EffectTemp = 0f;
		// all effects  that modify outgoing damage are called and change this methods
		// damage addition value and damage multipication value
		effects.triggerEffects(EffectTypes.damageDealtChange, this);
		if (damage0Flag_EffectTemp) {
			return 0f;
		}

		amount += damageAddition_EffectTemp;
		amount *= damageMultiplication_EffectTemp;

		return amount;
	}
	

	#endregion

	#region Hand of Cards
	
	#endregion

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider2D>();
		initButtonCallbacks();
		healthInit();
		initGrapics();
		aimObject = GetComponent(typeof(IGetAim)) as IGetAim;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region card helper functions
	public Vector2 get2dPostion() {
		return new Vector2(transform.position.x, transform.position.y);
	}
	
	/// <summary>
	/// aim of this object
	/// </summary>
	private IGetAim aimObject;
	public Vector2 getNormalizedAim(Vector2 startPoint) {
		if(aimObject == null) {
			Debug.LogWarning("No aim in object!\n" + gameObject.GetInstanceID());
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
