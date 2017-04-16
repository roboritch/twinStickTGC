using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawns : MonoBehaviour {

	public EnamySpawner[] spawners;

	private bool disabled = false;
	private void spawnEnamies() {
		foreach (var spawn in spawners) {
			spawn.spawnEnamy();
		}
		disabled = true;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(!disabled){
			Actor actor = coll.GetComponent<Actor>();
			if(actor != null) {
				if(actor.Team == DamageSources.player1) {
					spawnEnamies();
				}
			}
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
