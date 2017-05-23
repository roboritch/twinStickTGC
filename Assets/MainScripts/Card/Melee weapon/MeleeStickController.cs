using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class MeleeStickController : MonoBehaviour {
	private HingeJoint2D joint;
	private Rigidbody2D physicsBody;


	// Use this for initialization
	void Awake () {
		joint = GetComponent<HingeJoint2D>();
		physicsBody = GetComponent<Rigidbody2D>();
	}


	public bool isJointLocked() {
		return physicsBody.freezeRotation;
	}

	public void lockJoint() {
		physicsBody.freezeRotation = true;
	}

	public void freeJoint() {
		physicsBody.freezeRotation = false;
	}

	private bool swinging = false;

	private float swingLengthMax_seconds = 1f;
	private float swingLengthLeft_seconds;

	private float wantedAngularSpeed = 10f;

	public void startSwing() {
		JointMotor2D motor = new JointMotor2D() {
			motorSpeed = wantedAngularSpeed
		};
		joint.motor = motor;
		swingLengthLeft_seconds = swingLengthMax_seconds;
		swinging = true;
	}

	public void endSwing() {

	}

	private void swingObject() {
		if(swingLengthLeft_seconds > 0) {
			swingLengthLeft_seconds -= Time.fixedDeltaTime;
		} else {
			joint.useMotor = false;
			swinging = false;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(swinging)
			swingObject();
	
	}
}
