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

	//all connections to KeyEvents should be here and be disable
	#region button callbacks
	[SerializeField] private bool playerControled = false;
	private void initButtonCallbacks() {
		if (playerControled) { 
			KeyEvents.Instance.buttonCallbackFunctions.mainAction += mainActionPressed;
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
	
	#endregion

	#region Hand of Cards
	
	#endregion

	void Awake() {
		collider = GetComponent<Collider2D>();
		initHealth();
		initGrapics();
		initEffects();
		aimObject = GetComponent(typeof(IGetAim)) as IGetAim;
	}

	// Use this for initialization
	void Start () {
		initButtonCallbacks();
	}
	
	// Update is called once per frame
	void Update () {
		updateSpriteDamage();
	}

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
