using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour, IGetAim {

	protected Vector2 aimLocation;
	public void getAim(out Vector2 aimLocation) {
		aimLocation = this.aimLocation;
	}

	public virtual void setAim(Vector2 newAimLocation_WorldSpace) {
		aimLocation = newAimLocation_WorldSpace;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
