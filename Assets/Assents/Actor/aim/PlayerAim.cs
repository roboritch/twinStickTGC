using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : Aim {

	#region Reticle
	[SerializeField]
	private GameObject reticle;

	private Transform aimLocationObject;
	[SerializeField]
	private float layerOfReticle = -.1f;

	/// <summary>
	/// moves reticale based on mouse location
	/// </summary>
	private void updateReticalPosition() {
		Vector3	aimLocation3d = Input.mousePosition;
		aimLocation3d = Camera.main.ScreenToWorldPoint(aimLocation3d);
		aimLocation3d.z = layerOfReticle;
		aimLocationObject.position = aimLocation3d;
		aimLocation = aimLocation3d;
	}
	#endregion


	#region look at function
	private void lookAtRedicle() {
		Vector3 place = aimLocationObject.transform.position;
		place.z = transform.position.z;
		transform.up = place - transform.position;
	}
	#endregion

	#region camera control
	public Camera cam;
	[SerializeField] private float camMoveSpeed = 1f;
	private void updateCameraPos() {
		Vector3 newCamPos = transform.position;

		newCamPos.z = -2f;
		cam.transform.position = Vector3.Lerp(cam.transform.position, newCamPos,camMoveSpeed);
	}

	#endregion


	// Use this for initialization
	void Start () {
		KeyEvents.Instance.constrainMouseToScreen(true);
		aimLocationObject = Instantiate(reticle).GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
		updateReticalPosition();
		lookAtRedicle();
		updateCameraPos();
    }


}
