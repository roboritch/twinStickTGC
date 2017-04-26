using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyTrigger : MonoBehaviour {

	[SerializeField]
	private ShowOnTrigger objectToTrigger;
	
	void OnDestroy() {
		if(PauseMenuHandler.Instance.isQuiting == false && objectToTrigger != null) {
			objectToTrigger.trigger();
		}
	}
}
