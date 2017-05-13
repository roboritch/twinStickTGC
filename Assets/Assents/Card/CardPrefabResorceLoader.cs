using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabResorceLoader : Singleton<PrefabResorceLoader> {

	//TODO look up ways to improve this (not sure how good folder structure is
	#region Card Icon loading
	private Dictionary<string, Sprite> spriteResorces = new Dictionary<string, Sprite>();

	/// <summary>
	/// load the sprite from a dictionary of sprite resources 
	/// </summary>
	/// <param name="resorcePathname">CardClassName + / should be input here</param>
	/// <returns>returns null if sprite not found</returns>
	public Sprite loadSprite(string resorcePathname) {
		Sprite sprite;
		if (spriteResorces.TryGetValue(resorcePathname, out sprite)) {
			return sprite;
		} else { //get resource from folder
			sprite = Resources.Load<Sprite>(resorcePathname);
			if (sprite == null) {
				Debug.LogWarning("no sprite found with that name");
				return null;
			}
			spriteResorces.Add(resorcePathname, sprite);
			return sprite;
		}
	}
	#endregion

	#region text loading 
	private Dictionary<string, TextAsset> textResorces = new Dictionary<string, TextAsset>();

	/// <summary>
	/// load any text asset from the resource folder
	/// </summary>
	/// <param name="resorcePathname">CardClassName + / should be input here</param>
	/// <param name="fileName"></param>
	/// <returns></returns>
	public TextAsset loadTextAsset(string resorcePathname) {
		TextAsset textAsset;
		if(textResorces.TryGetValue(resorcePathname, out textAsset)) {
			return textAsset;
		} else { //get resource from folder
			textAsset = Resources.Load<TextAsset>(resorcePathname);
			if(textAsset == null) {
				Debug.LogWarning("no text asset found with that name");
				return null;
			}
			textResorces.Add(resorcePathname, textAsset);
			return textAsset;
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
	/// load a resource from disk and store it in ram for reuse (automatic)
	/// </summary>
	/// <param name="resorcePathname">the path to the resource</param>
	/// <returns>uninstantiated Prefab of GameObject, null if resource not found</returns>
	public GameObject loadPrefab(string resorcePathname) {
		GameObject prefab;
		if (prefabResorces.TryGetValue(resorcePathname,out prefab)) {
			return prefab;
		} else { //get resource from folder
			prefab = Resources.Load<GameObject>(resorcePathname);
			if (prefab == null) {
				return null;
			}
			prefabResorces.Add(resorcePathname, prefab);
			return prefab;
		}
	}

	/// <summary>
	/// cache a prefab from the resource folder to the prefabResorces Dictionary 
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

	public void preLoadAllCards() {
		IEnumerable<Card> allCards = ReflectiveEnumerator.GetEnumerableOfType<Card>();
		foreach(Card card in allCards) {
			//card base classes with this class name are not real cards and should be ignored
			if(card.GetType().Name.Contains("Base_")) {
				continue;
			}
			//this method will call this class to cash all resources the card requires to function
			card.cacheResorces();
		}
	}


}
