using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour, IGetAim {
	[SerializeField] private GameObject reticle;

	public void getAim(out Vector2 aimLocation) {
		Vector3 pos = aimLocationObject.getPosition();
		aimLocation.x = pos.x;
		aimLocation.y = pos.y;
	}

	private Reticle aimLocationObject;

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
		aimLocationObject = Instantiate(reticle).GetComponent<Reticle>();
    }
	
	// Update is called once per frame
	void Update () {
		lookAtRedicle();
		updateCameraPos();
    }


}
