using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	#region over time effects

	#endregion

	#region Hand of Cards
	
	#endregion

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider2D>();
		initButtonCallbacks();
		healthInit();
		initGrapics();
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
	#endregion

}
