using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ProjectilePrefabCreator : EditorWindow {

	private string effectsPath = "Assents/Effects";
	private string effectsName = "ListOfEffects";

	void OnEnable() {
			
	}

	[MenuItem("CardCrator/create projectile")]
	static void Init() {
		
		ProjectilePrefabCreator window = (ProjectilePrefabCreator)GetWindow(typeof(ProjectilePrefabCreator));
		window.Show();
	}

	private void Awake() {
		updatePosibleEffectsList();
	}
	
	private string projectileFilePath;
	private string projectileName;
	private float damage;
	private DamageTypes damageType;
	private EffectsList effectsApplyedOnDamage;
	private Sprite projectileSprite;

	void OnGUI() {
		if(GUILayout.Button("update list of effects and there properties")) {
			updatePosibleEffectsList();
		}
		projectileFilePath = EditorGUILayout.TextField("current path", projectileFilePath);
		if(GUILayout.Button("set prefab location based on selected Folder")) {
			projectileFilePath = getPathToSelection();
		}

		GUILayout.Label("Projectile stats");
		projectileName = EditorGUILayout.TextField("Projectile Name", projectileName);
		
		damage = EditorGUILayout.FloatField("damage",damage);
		damageType = (DamageTypes)EditorGUILayout.EnumMaskField("damage type",damageType);
		selectedEffect = EditorGUILayout.Popup(selectedEffect,possibleEffectsListNames);
		if(GUILayout.Button("add effect (applied on contact)")){
			chosenEffects.Add(selectedEffect);
		}

		foreach(int i in chosenEffects) {
			EditorGUILayout.LabelField(possibleEffectsList[i].effectClassName);
			for(int x = 0; x < possibleEffectsList[i].propertyName.Length; x++) {
				string prameTypeName = possibleEffectsList[i].valueTypeName[x];
				if(prameTypeName == typeof(int).Name) {
					possibleEffectsList[i].value[x] = EditorGUILayout.IntField(possibleEffectsList[i].propertyName[x], int.Parse(possibleEffectsList[i].value[x])).ToString();
				} else if(prameTypeName == typeof(float).Name) {
					possibleEffectsList[i].value[x] = EditorGUILayout.FloatField(possibleEffectsList[i].propertyName[x], float.Parse(possibleEffectsList[i].value[x])).ToString();
				} else if(prameTypeName == typeof(bool).Name) {
					possibleEffectsList[i].value[x] = EditorGUILayout.Toggle(possibleEffectsList[i].propertyName[x], bool.Parse(possibleEffectsList[i].value[x])).ToString();
				} else if(GetEnumType(prameTypeName).IsEnum) {
					Enum enumValue =  (Enum)Enum.ToObject(GetEnumType(prameTypeName), possibleEffectsList[i].value[x]);
					possibleEffectsList[i].value[x] = EditorGUILayout.EnumPopup(prameTypeName, enumValue).ToString();
				}
			}
		}

		projectileSprite = EditorGUILayout.ObjectField(projectileSprite, typeof(Sprite),false) as Sprite;
		


		if(GUILayout.Button("create projectile prefab")) {
			GameObject projectileObject = new GameObject(projectileName);
		}
	}

	private List<int> chosenEffects = new List<int>(); 

	private int selectedEffect;
	private string[] possibleEffectsListNames;
	private EffectProperties[] possibleEffectsList;
	private void updatePosibleEffectsList() {
		IEnumerable<Effect> possibleEffectEnumerable = ReflectiveEnumerator.GetEnumerableOfType<Effect>(new object[0]);
		int possibleEffectsCount = possibleEffectEnumerable.Count();
		possibleEffectsListNames = new string[possibleEffectsCount];
		possibleEffectsList = new EffectProperties[possibleEffectsCount];

		int index = 0;
		foreach(Effect effect in possibleEffectEnumerable) {
			EffectProperties properties = effect.getEffectPropertiesStructure(false);
			possibleEffectsListNames[index] = properties.effectClassName;
			possibleEffectsList[index++] = properties;
		}		

	}


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

	//https://stackoverflow.com/questions/25404237/how-to-get-enum-type-by-specifying-its-name-in-string
	public static Type GetEnumType(string enumName) {
		foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			var type = assembly.GetType(enumName);
			if(type == null)
				continue;
			if(type.IsEnum)
				return type;
		}
		return null;
	}
}

public static class ReflectiveEnumerator {
	static ReflectiveEnumerator() { }

	public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class {
		List<T> objects = new List<T>();
		foreach(Type type in
			Assembly.GetAssembly(typeof(T)).GetTypes()
			.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
			objects.Add((T)Activator.CreateInstance(type, constructorArgs));
		}
		return objects;
	}
}

/// <summary>
/// the projectile object will use this struct to add effects to 
/// the things it effects at runtime
/// </summary>
[System.Serializable]
public struct EffectsList {
	public EffectsList(bool initLists) {
		effectProperties = new List<EffectProperties>();
	}
	public List<EffectProperties> effectProperties;
}

/*AutocompleteSearch example for effect finding
public class AutocompleteSearch : MonoBehaviour {

	List<string> words = newList<string>();
	string myText = "";
	publicTextMesh textAutocomplete;
	void Start() {
		words.Add("Maria Martinez");
		words.Add("Maria Gonzales");
		words.Add("Mario Gonzales");
	}

	void OnGUI() {
		string oldString = myText;
		myText = GUI.TextField(newRect(10, 10, 200, 20), myText);
		if(!string.IsNullOrEmpty(myText) && myText.Length > oldString.Length) {
			List<string> found = words.FindAll(w => w.StartsWith(myText));
			if(found.Count > 0) {
				//myText = found[0];
				textAutocomplete.text = found[0];
				print(found.Count);
			}
		}
	}
}
*/
