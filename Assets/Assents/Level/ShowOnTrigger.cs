using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnTrigger : MonoBehaviour , ITriggerable {

	public void trigger() {
		GetComponent<MeshRenderer>().enabled = true;
	}
}

public interface ITriggerable {
	void trigger();
}
