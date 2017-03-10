﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Movment : MonoBehaviour {
	private Collider2D colider;
	private Rigidbody2D rigidBody;

	private void initComponents() {
		colider = GetComponent<Collider2D>();
		rigidBody = GetComponent<Rigidbody2D>();
	}



	#region key binds
	private void initKeyCallBacks() {
		KeyEvents.Instance.buttionCallbackFunctions.moveUp = moveUp;
		KeyEvents.Instance.buttionCallbackFunctions.moveDown = moveDown;
		KeyEvents.Instance.buttionCallbackFunctions.moveLeft = moveLeft;
		KeyEvents.Instance.buttionCallbackFunctions.moveRight = moveRight;
	}

	public void moveUp() {
		movmentWantedVertacal = maxSpeed;
	}
	public void moveDown() {
		movmentWantedVertacal = -maxSpeed;
	}
	public void moveLeft() {
		movmentWantedHorizontal = -maxSpeed;
	}
	public void moveRight() {
		movmentWantedHorizontal = maxSpeed;
	}
	#endregion

	#region move
	[SerializeField] private float maxSpeed = 10f;
	[SerializeField] private float maxAccelerationPerSec = .1f;
	[SerializeField] private float decelerationMultiplyer = 3f;

	public Player.floatModifyer maxSpeedModifyers;

	public float getMaxSpeed() {
		float modifyedSpeed = maxSpeed;
		modifyedSpeed += maxSpeedModifyers(maxSpeed);

		return maxSpeed;
	}
	/// <summary>
	/// this var should be used when an ability affects player movment
	/// </summary>
	private float userInputMultiplyer;
	private float movmentWantedVertacal = 0;
	private float movmentWantedHorizontal = 0;

	private void resetMovment() {
		movmentWantedHorizontal = 0;
		movmentWantedVertacal = 0;
	}

	/// <summary>
	/// changes the velocity of an object based on inputs
	/// must be called in Update if at all.
	/// </summary>
	private void applyMovment() {
		Vector2 wantedVelocety = new Vector2(movmentWantedHorizontal,movmentWantedVertacal);
		Vector2 currentVelocety = rigidBody.velocity;

		float accelerationAmount = Time.timeScale * maxAccelerationPerSec * Time.deltaTime; //make movment independent of framrate

		if (currentVelocety.magnitude > maxSpeed) { //if the object velocity is greater than max slow it quicly
			accelerationAmount *= 20;
		}

		Vector2 newVelocity = Vector2.Lerp(currentVelocety, wantedVelocety, accelerationAmount);
		if(newVelocity.sqrMagnitude < currentVelocety.sqrMagnitude) { //slow down faster on deceleration
			print("slowing");
			newVelocity = Vector2.Lerp(currentVelocety, wantedVelocety, accelerationAmount * decelerationMultiplyer);
		}

		rigidBody.velocity = newVelocity;
	}

	#endregion


	// Use this for initialization
	void Start () {
		initKeyCallBacks();
		initComponents();
	}
	
	// is called at a consistent rate per second
	void FixedUpdate() {
	
	}

	// Update is called once per frame
	void Update () {
		applyMovment();
		resetMovment();
	}
}