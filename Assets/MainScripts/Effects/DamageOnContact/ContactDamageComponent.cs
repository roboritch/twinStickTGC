using UnityEngine;
using System.Collections;

public class ContactDamageComponent : MonoBehaviour {

	// set to -1 so if this component is uninitialized it is detectable
	protected float maxDamageInterval_seconds;
	protected float damageInterval_seconds;

	protected bool damageActive = false;
	public void setActive() {
		damageActive = true;
	}

	/// <summary>
	/// this must be called when this component is created;
	/// </summary>
	/// <param name="amount">how much damage does this do</param>
	/// <param name="type"></param>
	/// <param name="interval_seconds"></param>
	public void initDamage(Actor cardUser, float amount,DamageTypes type,float interval_seconds) {
		user = cardUser;
		damageAmount = amount;
		damageType = type;
		maxDamageInterval_seconds = interval_seconds;
		damageInterval_seconds = 0;
	}

	/// <summary>
	/// used to get damage modifiers
	/// </summary>
	protected Actor user;
	/// <summary>
	/// damage dealt on contact per second
	/// </summary>
	protected float damageAmount;
	protected DamageTypes damageType;

	protected new Collider2D collider;
	void OnCollisionEnter2D(Collision2D coll) {
		if (damageActive) {
			IDamageable actor = coll.collider.GetComponent<IDamageable>();
			if (actor != null) {
				if (actor.ignoreDamage(user.Team, damageType)) {
					return;
				}
				if(actor.takeDamage(user.effects.modifyDamage(damageAmount,damageType,false), damageType, user.Team)) {
					resetDamageTimer();
				}
			}
		}
	}

	protected void activateDamage() {
		damageActive = true;
		//toggles the collider to get OnCollisionEnter2D calls from already contacted colliders
		//WARNING this will call OnCollisionEnter2D for all components on this actor (if they use that method)
		collider.enabled = false;
		collider.enabled = true;
		updateAnimation();
	}

	protected void resetDamageTimer() {
		damageInterval_seconds = maxDamageInterval_seconds;
		damageActive = false;
		updateAnimation();
    }

	private Animator damageAnimationControler;
	/// <summary>
	/// update the damage animation 
	/// </summary>
	private void updateAnimation() {
		if(!animationLoadError)
		if (damageActive) {
			//show spikes
			damageAnimationControler.gameObject.SetActive(true);
		} else {
			//hide spikes
			damageAnimationControler.gameObject.SetActive(false);
		}
	}

	bool animationLoadError = false;
	private string animationName = "spinning part";
	private void loadAnimationPrefab() {
		GameObject animationPrefab = Resources.Load<GameObject>(GetType().Name + "/" + animationName);
		if (animationPrefab == null) {
			Debug.LogError("no animation prefab on this object ID:" + transform.GetInstanceID() + "\n"
				+ "look for " + GetType().Name + "add find the prefab for it");
			animationLoadError = true;
			return;
		}
		GameObject obj = Instantiate(animationPrefab);
		obj.transform.GetOrAddComponent<RotationIndependentFollow>().objectFollowing = transform;
		damageAnimationControler = obj.GetComponentInChildren<Animator>();


	}

	void Start() {
		if(maxDamageInterval_seconds == -1f) {
			Debug.LogError("uninitialized ContactDamageComponent, ID:" + transform.GetInstanceID());
		}
		collider = GetComponent<Collider2D>();
        if (collider == null) {
			Debug.LogError("no collider on ContactDamageComponent ID:" + transform.GetInstanceID());
		}
		loadAnimationPrefab();
    }

	void Update() {
		if (!damageActive) {
			if (damageInterval_seconds > 0) {
				damageInterval_seconds -= Time.deltaTime;
			} else {
				activateDamage();
            }
		}
	}
}
