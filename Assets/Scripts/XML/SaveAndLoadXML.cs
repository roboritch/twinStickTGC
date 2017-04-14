using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System;

public static class SaveAndLoadXML{
	/// <summary>
	/// Gets the base file path (game path with / ending).
	/// </summary>
	/// <returns>The base file path.</returns>
	public static string getBaseFilePath(){
		return Application.dataPath + "/";
	}

	/// <summary>
	/// Saves the struct as an XML file.
	/// </summary>
	/// <param name="filePath">File path, .xml will be apended to the end of this string</param>
	/// <param name="savingFile">Saving file.</param>
	/// <typeparam name="T">Struct type to save (Must be XML formated).</typeparam>
	public static void saveXML<T>(string filePath, T savingFile){
		filePath += ".xml";
		if (File.Exists(filePath)) {
			File.Delete(filePath);
		} else {
			//makes sure folder path exists
			if (!Directory.Exists(filePath)) {
				string fileDir = filePath;
				while (!fileDir.EndsWith("/") && fileDir.Length != 0) {
					fileDir = fileDir.Remove(fileDir.Length - 1);
				}
				Directory.CreateDirectory(fileDir);

			}
		}

		FileStream stream = null;
		try{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			stream = new FileStream(filePath, FileMode.CreateNew);
			// New grid info to disk here.
			serializer.Serialize(stream, savingFile);
			stream.Close();
		} catch(Exception ex){
			Debug.LogError(ex.ToString());
			if(stream != null)
				stream.Close();
		}
	}

	/// <summary>
	/// Loads the XM.
	/// </summary>
	/// <returns><c>true</c>, if XML was loaded, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path with file name.</param>
	/// <param name="fileOut">File out.</param>
	/// <typeparam name="T">Struct type</typeparam>
	public static bool loadXML<T>(string filePath, out T fileOut){
		filePath += ".xml"; 
		fileOut = default(T);
		if(!File.Exists(filePath)){
			Debug.Log("no file with that name exists in the location\n" + filePath);
			return false;
		}
			
		FileStream stream = null;

		try{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			stream = new FileStream(filePath, FileMode.Open);
			fileOut = (T)serializer.Deserialize(stream);
			stream.Close();
		} catch(Exception ex){
			if(stream != null)
				stream.Close();
			Debug.LogWarning("struct load failed, error:\n" + ex);
			return false;
		}
		return true;
	}



	#region example xml
	[Serializable]
	public struct GridInfo{
		[XmlArrayItemAttribute("enabled")]
		[XmlArrayAttribute("block_enabled_list")]
		public bool[] enabled;

		[XmlArrayItemAttribute("Spawn")]
		[XmlArrayAttribute("SpawnSpot_list")]
		public bool[] isSpawnSpot;

		[XmlArrayItemAttribute("AI")]
		[XmlArrayAttribute("AI_enabled_list")]
		public bool[] isAI;

		[XmlArrayItemAttribute("t_numb")]
		[XmlArrayAttribute("team_numbers")]
		public int[] team;

		[XmlElement("grid_size")]
		// The width and height (x, y) value of the gridblock
		public int gridSize;
	}
	#endregion

}
