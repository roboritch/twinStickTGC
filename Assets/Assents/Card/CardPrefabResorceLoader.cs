using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrefabResorceLoader : Singleton<CardPrefabResorceLoader> {

	#region Card Icon loading
	private Dictionary<string, Sprite> spriteResorces = new Dictionary<string, Sprite>();

	/// <summary>
	/// naming convention for icon to be in CardClassName/
	/// </summary>
	/// <param name="resorcePathname">CardClassName/ should be input here</param>
	/// <returns></returns>
	public Sprite loadSprite(string resorcePathname) {
		Sprite sprite;
		if (spriteResorces.TryGetValue(resorcePathname, out sprite)) {
			return sprite;
		} else { //get resource from folder
			sprite = Resources.Load<Sprite>(resorcePathname + "icon");
			if (sprite == null) {
				Debug.LogWarning("no sprite found with that name");
				return null;
			}
			spriteResorces.Add(resorcePathname, sprite);
			return sprite;
		}
	}

	#endregion


	#region Prefab loading
	/// <summary>
	/// key is pathname
	/// prefab is stored
	/// </summary
	private Dictionary<string, GameObject> prefabResorces = new Dictionary<string, GameObject>();

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
	#endregion
}
