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
	/// moves reticle based on mouse location
	/// </summary>
	private void updateReticlePosition() {
		Vector3	aimLocation3d = Input.mousePosition;
		aimLocation3d = Camera.main.ScreenToWorldPoint(aimLocation3d);
		aimLocation3d.z = layerOfReticle;
		aimLocationObject.position = aimLocation3d;
		setAim(aimLocation3d);
	}
	#endregion

	#region camera control
	public Camera cam;
	[SerializeField] private float camMoveSpeed = 1f;
	/// <summary>
	/// updates the camera position to fallow the player
	/// TODO update with smooth flowing and action fallowing 
	/// </summary>
	private void updateCameraPos() {
		Vector3 newCamPos = transform.position;

		newCamPos.z = -2f;
		cam.transform.position = Vector3.Lerp(cam.transform.position, newCamPos,camMoveSpeed);
	}

	#endregion

	void Awake() {
		if(cam == null) {
			cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}
	}

	void Start () {
		KeyEvents.Instance.constrainMouseToScreen(true);
		aimLocationObject = Instantiate(reticle).GetComponent<Transform>();
	}
	
	void Update () {
		updateCameraPos();
		updateReticlePosition();
		lookAtAimLocation();
	}


}
