using UnityEngine;
using System.Collections;

public class ContactDamageComponent : MonoBehaviour {

	private float damageAmount;

	void OnTriggerEnter2D(Collider2D coll) {
		Actor actor = coll.GetComponent<Actor>();
		if (actor != null) {
			
		}
	}

	//remove actor from area
	void OnTriggerExit2D(Collider2D coll) {
		Actor actor = coll.GetComponent<Actor>();
		if (actor != null) {
			actorsInArea.Remove(coll);
		}
	}

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
