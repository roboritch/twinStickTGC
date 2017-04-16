using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationIndependentFollow : MonoBehaviour {

	public Transform objectFollowing;

	// Update is called once per frame
	void Update () {
		if(objectFollowing != null) {
			transform.position = objectFollowing.position;
		} else {
			UnityExtentionMethods.destoryAllChildren(transform);
		}
	}
}
