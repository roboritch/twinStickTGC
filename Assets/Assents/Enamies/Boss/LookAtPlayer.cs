using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = transform.position;
		Vector2 actorPos = player.transform.position;
		Vector3 relativeActorPos = (pos - actorPos);

		float ang = Vector2.Angle(actorPos, relativeActorPos);
		Vector3 cross = Vector3.Cross(actorPos, relativeActorPos);

		if (cross.z > 0)
			ang = 360 - ang;

		//	float zRot = Mathf.Acos( Vector2.an  ) * (180 / Mathf.PI);
		transform.eulerAngles = new Vector3(0, 0, -ang);

	}
}
