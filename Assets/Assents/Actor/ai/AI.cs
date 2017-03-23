using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movment))]
[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Aim))]
public class AI : MonoBehaviour {
	private Actor actor;
	private Movment movmentControler;
	private Aim aim;

	private void initAI() {
		actor = GetComponent<Actor>();
		movmentControler = GetComponent<Movment>();
		aim = GetComponent<Aim>();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
