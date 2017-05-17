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
	/// Application.dataPath + "/" + folderPath + "/"
	/// </summary>
	/// <param name="folderPath"></param>
	/// <returns></returns>
	public static string getBaseFilePath(string folderPath) {
		return Application.dataPath + "/" + folderPath + "/";
	}

	/// <summary>
	/// Application.dataPath + "/" + folderPath + "/"
	/// </summary>
	/// <param name="folderPath"></param>
	/// <param name="fileName"></param>
	/// <returns></returns>
	public static string getBaseFilePath(string folderPath, string fileName) {
		return Application.dataPath + "/" + folderPath + "/" + fileName;
	}

	/// <summary>
	///  Application.dataPath + "/" + "Resources/" + folderPath + "/" + fileName
	/// </summary>
	/// <param name="folderAndItemInResource"></param>
	/// <returns></returns>
	public static string getResourcePath(string folderPath, string fileName) {
		return Application.dataPath + "/" + "Resources/" + folderPath + "/" + fileName;
	}

	/// <summary>
	///  Application.dataPath + "/" + "Resources/" + folderAndItemInResource 
	/// </summary>
	/// <param name="folderAndItemInResource"></param>
	/// <returns></returns>
	public static string getResourcePath(string folderAndItemInResource) {
		return Application.dataPath + "/" + "Resources/" + folderAndItemInResource;
	}

	#region save load string
	/// <summary>
	/// save a struct to a string 
	/// this can also be used for primitive types
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <param name="obj"></param>
	/// <param name="json"></param>
	/// <returns></returns>
	public static bool saveStructToString<K>(K obj, out string json) {
		try {
			json = JsonUtility.ToJson(obj);
		} catch(System.Exception) {
			Debug.LogError("struct save failed ");
			json = "{ }";
			return false;
		}
		return true;
	}

	/// <summary>
	/// load a struct from a string
	/// the type of the object must be known by the calling method
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <param name="obj"></param>
	/// <param name="json"></param>
	/// <returns>true on load success</returns>
	public static bool loadStructFromString<K>(out K obj, string json) {
		try {
			obj = JsonUtility.FromJson<K>(json);
		} catch(System.Exception) {
			Debug.LogError("struct load failed");
			obj = default(K);
			return false;
		}
		return true;
	}
	#endregion

	#region save load Struct
	/// <summary>
	/// save a struct using json
	/// notice: any structs contained in K must be set as [System.Serializable] 
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <param name="path"></param>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static bool saveStruct<K>(string path, K obj) {
		path += ".json";
		try {
			File.WriteAllText(path, JsonUtility.ToJson(obj));
		} catch(System.Exception) {
			Debug.LogError("struct save failed " + path);
			return false;
		}

		return true;
	}

	public static bool saveStruct<K>(string path, K obj,bool humanReadable) {
		path += ".json";
		try {
			File.WriteAllText(path, JsonUtility.ToJson(obj,humanReadable));
		} catch(System.Exception) {
			Debug.LogError("struct save failed " + path);
			return false;
		}

		return true;
	}

	public static bool loadStruct<K>(string path,out K obj) {
		path += ".json";
		try {
			obj = JsonUtility.FromJson<K>(File.ReadAllText(path));
		} catch(System.Exception) {
			Debug.LogError("struct load failed " + path);
			obj = default(K);
			return false;
		}

		return true;
	}
	#endregion

	#region Save Load Dictionary
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
			Debug.LogError("json Dictionary save failed, path: " + path);
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
		path += ".json";
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
	#endregion


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
