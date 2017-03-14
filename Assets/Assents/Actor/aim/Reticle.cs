using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour {

	/// <summary>
	/// what layer is the sprite on
	/// </summary>
	[SerializeField]
	private float hight = -.1f;

	private void updatePosition() {
		Vector3 reticlePos = Input.mousePosition;
		reticlePos = Camera.main.ScreenToWorldPoint(reticlePos);
		reticlePos.z = hight;
		transform.position = reticlePos;
	}

	public Vector3 getPosition() {
		return transform.position;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updatePosition();
	}
}
