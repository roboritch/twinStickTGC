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

	#region look at function
	public void lookAtAimLocation() {
		Vector3 place = aimLocation;
		place.z = transform.position.z;
		transform.up = place - transform.position;
	}
	#endregion

}
