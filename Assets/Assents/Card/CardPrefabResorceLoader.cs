using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrefabResorceLoader : Singleton<CardPrefabResorceLoader> {

	/// <summary>
	/// key is pathname
	/// prefab is stored
	/// </summary
	private Dictionary<string, GameObject> prefabResorces;

	/// <summary>
	/// load a resorce from disk and store it in ram for reuse (automatic)
	/// </summary>
	/// <param name="resorcePathname">the path to the resorce</param>
	/// <returns>uninstateated Prefab of GameObject, null if resorce not found</returns>
	public GameObject loadPrefab(string resorcePathname) {
		GameObject prefab;
		if (prefabResorces.TryGetValue(resorcePathname,out prefab)) {
			return prefab;
		} else { //get resorce from folder
			prefab = Resources.Load<GameObject>(resorcePathname);
			if (prefab == null) {
				return null;
			}
			prefabResorces.Add(resorcePathname, prefab);
			return prefab;
		}
	}	

	/// <summary>
	/// 
	/// </summary>
	/// <param name="resorcePathname"></param>
	/// <returns></returns>
	public bool cashePrefab(string resorcePathname) {
		if (prefabResorces.ContainsKey(resorcePathname)) {
			return true;
		}
		GameObject prefab = Resources.Load<GameObject>(resorcePathname);
		if (prefab == null) {
			return false;
		}
		prefabResorces.Add(resorcePathname, prefab);
		return true;
	}
}
