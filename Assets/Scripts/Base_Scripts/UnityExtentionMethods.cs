using UnityEngine;
using System.Collections;

public static class UnityExtentionMethods{
	/// <summary>
	/// Destroys all children and the selected object.
	/// </summary>
	/// <param name="nextTransform">Next transform.</param>
	public static void destoryAllChildren(Transform nextTransform){
		foreach( Transform child in nextTransform ){
			destoryAllChildren(child);
		}
		MonoBehaviour.Destroy(nextTransform.gameObject);
	}


}
