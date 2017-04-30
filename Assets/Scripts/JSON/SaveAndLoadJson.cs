using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public static class SaveAndLoadJson {

	/// <summary>
	/// Gets the base file path (game path with / ending).
	/// </summary>
	/// <returns>The base file path.</returns>
	public static string getBaseFilePath() {
		return Application.dataPath + "/";
	}

	/// <summary>
	/// save a struct using json
	/// notice: any structs contained in K must be set as [System.Serializable] 
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <param name="path"></param>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static bool saveStruct<K>(string path, K obj) {
		try {
			File.WriteAllText(path, JsonUtility.ToJson(obj));
		} catch(System.Exception) {
			Debug.LogError("struct save failed " + path);
			return false;
		}

		return true;
	}

	public static bool loadStruct<K>(string path,out K obj) {
		try {
			obj = JsonUtility.FromJson<K>(File.ReadAllText(path));
		} catch(System.Exception) {
			Debug.LogError("struct load failed " + path);
			obj = default(K);
			return false;
		}

		return true;
	}

	/// <summary>
	/// save a dictionary with a struct as Value (making struct work)
	/// does not save hash values or ordering
	/// </summary>
	/// <typeparam name="K">a basic data type that can be converted to string using
	/// toString()</typeparam>
	/// <typeparam name="V">some struct or data structure that can be converted to json</typeparam>
	/// <param name="path"></param>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static bool saveDictionary<K,V>(string path, Dictionary<K, V> dic) {
		KeyValuePair<K, V>[] values = dic.ToArray();
		DictonaryWraper dw = new DictonaryWraper(values.Length);

		//convert values to json serialization
		for(int i = 0; i < values.Length; i++) {
			dw.key[i] = values[i].Key.ToString();
			dw.jsonValue[i] = JsonUtility.ToJson(values[i].Value);
		}

		try {
			//write to file
			File.WriteAllText(path, JsonUtility.ToJson(dw));
		} catch(System.Exception) {
			Debug.LogError("json Dic save failed, path: " + path);
			return false;
		}

		return true;
	}

	/// <summary>
	/// load a dictionary
	/// </summary>
	/// <typeparam name="K">a basic data type that can be converted to and from string using cast</typeparam>
	/// <typeparam name="V">some struct or data structure that can be converted to json</typeparam>
	/// <param name="path"></param>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static bool LoadDictionary<K,V>(string path, out Dictionary<K, V> dic) {
		DictonaryWraper dw;
		try {
			string json = File.ReadAllText(path);
			dw = JsonUtility.FromJson<DictonaryWraper>(json);
		} catch(System.Exception) {
			dic = new Dictionary<K, V>();
			return false;
		}

		try {
			dic = new Dictionary<K, V>();
			for(int i = 0; i < dw.key.Length; i++) {
				//convert key to value and load each V from json strings
				dic.Add((K)System.Convert.ChangeType(dw.key[i], typeof(K)), JsonUtility.FromJson<V>(dw.jsonValue[i]));
			}

		} catch(System.Exception) {
			Debug.LogError("could not convert from string to typeof(K)");
			dic = new Dictionary<K, V>();
			return false;
		}

		return true;
	}



	[System.Serializable]
	public struct DictonaryWraper {
		public DictonaryWraper(int arrayLengths) {
			key = new string[arrayLengths];
			jsonValue = new string[arrayLengths];
		}

		public DictonaryWraper(string[] key, string[] jsonValue) {
			this.key = key;
			this.jsonValue = jsonValue;
		}

		/// <summary>
		/// some string key
		/// </summary>
		public string[] key;
		/// <summary>
		/// some struct serialized to json string
		/// </summary>
		public string[] jsonValue;
	}



}
