using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyTrigger : MonoBehaviour {

	[SerializeField]
	private MonoBehaviour objectToTrigger;
	
	void OnDestroy() {
		if(objectToTrigger != null) {
			if(objectToTrigger is ITriggerable) {
				(objectToTrigger as ITriggerable).trigger();
			} else {
				Debug.LogWarning("tried to trigger non-Triggerable object " + transform.GetInstanceID() + "\n" +
					"OnDestroyTrigger instance ID:" + this.GetInstanceID());
			}
		} else {
			Debug.LogWarning("trigger object missing " + transform.GetInstanceID() + "\n" +
					"OnDestroyTrigger instance ID:" + this.GetInstanceID());
		}
	}
}
