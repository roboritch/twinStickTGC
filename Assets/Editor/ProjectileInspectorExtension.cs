using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System;

[CustomEditor(typeof(ProjectileBase))]
public class ProjectileInspectorExtension : Editor {

	private void OnEnable() {
		updatePosibleEffectsList();
	}

	//set and reset on updatePosibleEffectsList()
	private string[] possibleEffectsListNames;
	private EffectProperties[] possibleEffectsList;

	private void updatePosibleEffectsList() {
		IEnumerable<Effect> possibleEffectEnumerable = ReflectiveEnumerator.GetEnumerableOfType<Effect>(new object[0]);
		int possibleEffectsCount = possibleEffectEnumerable.Count();
		possibleEffectsListNames = new string[possibleEffectsCount];
		possibleEffectsList = new EffectProperties[possibleEffectsCount];

		int index = 0;
		foreach(Effect effect in possibleEffectEnumerable) {
			EffectProperties properties = effect.getEffectPropertiesStructure(true);
			possibleEffectsListNames[index] = properties.effectClassName;
			possibleEffectsList[index++] = properties;
		}



		if(index == 0) {
			Debug.LogWarning("no effects found!!");
		}
	}


	private int selectedEffect;
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		ProjectileBase myTarget = (ProjectileBase)target;

		selectedEffect = EditorGUILayout.Popup(selectedEffect, possibleEffectsListNames);
		if(GUILayout.Button("add effect (applied on contact)")) {
			Array.Resize(ref myTarget.effectsApplyedOnContact, myTarget.effectsApplyedOnContact.Length + 1);
			myTarget.effectsApplyedOnContact[myTarget.effectsApplyedOnContact.Length - 1] = possibleEffectsList[selectedEffect];

		}

		//TODO save and load serialized version of effects to the target
		//use this http://answers.unity3d.com/questions/610303/unity-systemobject-serialization.html
		foreach(EffectProperties effectValues in myTarget.effectsApplyedOnContact) {
			EditorGUILayout.LabelField(effectValues.effectClassName + " values");
			for(int x = 0; x < effectValues.propertyName.Length; x++) {
				string prameTypeName = effectValues.valueTypeName[x];
				if(prameTypeName == typeof(int).Name) {
					effectValues.value[x] = EditorGUILayout.IntField(effectValues.propertyName[x], (int)effectValues.value[x]);
				} else if(prameTypeName == typeof(float).Name) {
					effectValues.value[x] = EditorGUILayout.FloatField(effectValues.propertyName[x], (float)effectValues.value[x]);
				} else if(prameTypeName == typeof(bool).Name) {
					effectValues.value[x] = EditorGUILayout.Toggle(effectValues.propertyName[x], (bool)effectValues.value[x]);
				} else if(GetEnumType(prameTypeName).IsEnum) {
					//TODO see if menu has flags and change inspecter selection type
					Enum enumValue = (Enum)Enum.ToObject(GetEnumType(prameTypeName), effectValues.value[x]);
					effectValues.value[x] = EditorGUILayout.EnumPopup(prameTypeName, enumValue);
				}
			}
		}
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
