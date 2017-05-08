using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SetCardBaseStats : EditorWindow {

	private BasicCardAttributes cardAttrabutes;
	/// <summary>
	/// the folder structure in (but not including) Resources
	/// </summary>
	private string cardFilePath = "";

	[MenuItem("CardCrator/Set Card Stats")]
	static void Init() {
		SetCardBaseStats window = (SetCardBaseStats)GetWindow(typeof(SetCardBaseStats));
		window.Show();
	}

	private void Awake() {
		cardAttrabutes = default(BasicCardAttributes);
		cardAttrabutes.probabilityOfDraw = 1f;
	}


	private MonoScript card;
	private TextAsset existingCardAttr;
	void OnGUI() {
		card = (MonoScript)EditorGUILayout.ObjectField("card script", card, typeof(MonoScript), false);
		cardFilePath = GUILayout.TextField(cardFilePath);
		if(!cardFilePath.EndsWith("/")) {
			cardFilePath += "/";
		}

		if(GUILayout.Button("set prefab location based on cardClass")) {
			cardFilePath = card.GetClass().Name + "/";
			//default names
			cardAttrabutes.cardIconPath = cardFilePath + "icon";
			cardAttrabutes.cardArtPath = cardFilePath + "card art";
			cardAttrabutes.cardDescriptionPath = cardFilePath + "description";
			cardAttrabutes.cardName = card.GetClass().Name;

		}

		//load existing card attributes

		TextAsset tempChangeCheck = existingCardAttr;
		existingCardAttr = (TextAsset)EditorGUILayout.ObjectField("CardAttr", existingCardAttr, typeof(TextAsset), false);
		if(GUILayout.Button("update Existing CardAttr") || tempChangeCheck != existingCardAttr) {
			if(existingCardAttr != null) {
				SaveAndLoadJson.loadStructFromString(out cardAttrabutes, existingCardAttr.text);
				string[] filePath = AssetDatabase.GetAssetPath(existingCardAttr).Split(new char[] { '/' });
				int index = 0;
				while(index < filePath.Length) {
					if(filePath[index++] == "Resources") {
						break;
					}
				}
				cardFilePath = "";
				for(int i = index; i < filePath.Length-1; i++) {
					cardFilePath += filePath[i] + "/";
				}

			}

			//default names
			cardAttrabutes.cardIconPath = cardFilePath + "icon";
			cardAttrabutes.cardArtPath = cardFilePath + "card art";
			cardAttrabutes.cardDescriptionPath = cardFilePath + "description";
		}

		cardAttrabutes.cardName = EditorGUILayout.TextField("card displayed name", cardAttrabutes.cardName);
		cardAttrabutes.cardResorceCost = EditorGUILayout.FloatField("cost", cardAttrabutes.cardResorceCost);
		cardAttrabutes.cardReloadTime_seconds = EditorGUILayout.FloatField("slot reload time", cardAttrabutes.cardReloadTime_seconds);
		cardAttrabutes.removeOnDraw = EditorGUILayout.Toggle("removedOnDraw", cardAttrabutes.removeOnDraw);
		cardAttrabutes.probabilityOfDraw = EditorGUILayout.FloatField("draw probability", cardAttrabutes.probabilityOfDraw );
		cardAttrabutes.numberOfCardAllowedInDeck = EditorGUILayout.IntField("Numb Allowed In Deck", cardAttrabutes.numberOfCardAllowedInDeck);
		cardAttrabutes.cardElements = (DamageTypes)EditorGUILayout.EnumMaskPopup("setCardElements", cardAttrabutes.cardElements);
		cardAttrabutes.cardType = (CardTypes)EditorGUILayout.EnumMaskPopup("setCardType(s)", cardAttrabutes.cardType);


		GUILayout.Label("icon,card art, and card description (.txt)\nmust be added to the created folder");

		GUIStyle colorset = new GUIStyle(GUI.skin.button);
		if(timeLeftOnScreen_sec > 0) {
			colorset.normal.textColor = new Color(0f, 0.4f, 0f);
			timeLeftOnScreen_sec -= Time.deltaTime;
		} else
			colorset.normal.textColor = Color.black;

		if(GUILayout.Button("create card information", colorset) && cardFilePath != "/") {
			if(!Directory.Exists(cardFilePath))
				Directory.CreateDirectory(cardFilePath);

			//string printer = "";
			//SaveAndLoadJson.saveStructToString(cardAttrabutes,out printer);
			if(SaveAndLoadJson.saveStruct(SaveAndLoadJson.getResourcePath(cardFilePath + "CardAttr.json"), cardAttrabutes)) {
				timeLeftOnScreen_sec = timeOnScreenMax_sec;
			}
			AssetDatabase.Refresh();
		}

		
	}

	private float timeOnScreenMax_sec = 2f;
	private float timeLeftOnScreen_sec = 0;



	public static string getPathToSelection() {
		string path = "Assets";

		foreach(UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
			path = AssetDatabase.GetAssetPath(obj);
			if(!string.IsNullOrEmpty(path) && File.Exists(path)) {
				path = Path.GetDirectoryName(path);
				break;
			}
		}
		return path;
	}
}
