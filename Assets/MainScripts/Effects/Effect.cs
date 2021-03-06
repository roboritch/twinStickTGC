﻿

/// <summary>
/// All effects must have a constructor that takes no arguments (though
/// they can still have additional constructors that do take arguments 
/// this is due to editor scripts using reflection
/// </summary>
public abstract class Effect {
	public Effect(bool requiresCreator, bool mustBeInitalized) {
		this.requiresCreator = requiresCreator;
		this.effectMustBeInitalized = mustBeInitalized;
	}


	public bool canBeTriggered = false;
	/// <summary>
	/// used by the effect container to know what effect this is
	/// </summary>
	protected EffectTypes effectType;
	public EffectTypes getEffectType {
		get { return effectType; }
	}

	protected int usesLeft;
	/// <summary>
	/// read only: once equal to 0 this effect should be removed
	/// this is checked before any effect containers whenever apply is called
	/// </summary>
	public int NumberOfUsesLeft {
		get { return usesLeft; }
	}

	#region effect initialization
	/// <summary>
	/// set to true if the effect has a reference to the actor creating it
	/// this should be checked by all methods that create effects
	/// </summary>
	public readonly bool requiresCreator = false;
	/// <summary>
	/// set the creator of this effect 
	/// WARNING when using the stored reference check actor.isDead() to prevent null errors
	/// </summary>
	/// <param name="creator"></param>
	public abstract void setCreator(Actor creator);


	/// <summary>
	/// set to true if the effect must be initialized with the actor it is added to
	/// </summary>
	public readonly bool effectMustBeInitalized = false;
	/// <summary>
	/// whatever initialization the actor requires 
	/// WARNING this should only be called by the EffectsContainerComponent 
	/// and the actor should not be stored in a class variable
	/// </summary>
	/// <param name="actor"></param>
	public abstract void initalize(Actor actor);
	#endregion

	/// <summary>
	/// apply some effect to the actor (damage, moment changes, ext)
	/// called by EffectsContainerComponent on trigger or time intervals
	/// </summary>
	/// <param name="applyTo"></param>
	public abstract void apply(Actor applyTo);

	/// <summary>
	/// called by Effect container to fully remove this effect
	/// </summary>
	public abstract void removeEffect();

	/// <summary>
	/// called to cache resources of effect in ram
	/// </summary>
	public abstract void cacheResorces();


	/*all properties that are set using the below method should be
	 be defined in a region marked "effect properties" and be
	 defined in the same order as the below methods implement them*/

	/// <summary>
	/// the structure of the struct must be gotten from getEffectPropertiestructer
	/// use the below python script to set the values from the 
	/// required variables (reads the clipboard)
	/// does not work for enums, use Variable = (E)Enum.Parse(typeof(E),properties.value[property number]);
	/// </summary>
	/// <param name="properties"></param>
	public abstract void setEffectProperties(EffectProperties properties);
	/*
from tkinter import Tk
import re
reader = Tk()
s = reader.clipboard_get()
s = re.sub('\t', '', s)
s = re.sub(';','',s)
s = s.split('\n')
while s.__contains__(''):
    s.remove('')
for subst in s:
    if '//' in subst[:6]:
        s.remove(subst)

set1 = ''
numb = 0
lines = []

for ln in s:
    ln = ln.split()
    ln = ln[1:2+1] #only  the var type and var name are wanted
    set1 = ln[1] + ' = ' + ln[0] + '.Parse(properties.value['+ str(numb) +']);'
    numb += 1
    lines.append(set1 + '\n')
final = ''.join(lines)
reader.clipboard_clear()
reader.clipboard_append(final)
 */

	/// <summary>
	/// returns the structure of this effects modifiable properties
	/// use the below python script to setup the effectProperties from the 
	/// required variables (reads the clipboard)
	/// </summary>
	/// <param name="forGUI">add extra info not needed at runtime</param>
	/// <returns></returns>
	public abstract EffectProperties getEffectPropertiesStructure(bool forGUI);
	/* 
from tkinter import Tk
import re
reader = Tk()
s = reader.clipboard_get()
s = re.sub('\t', '', s)
s = re.sub(';','',s)
s = s.split('\n')
while s.__contains__(''):
    s.remove('')
for subst in s:
    if '//' in subst[:6]:
        s.remove(subst)
set1 = ''
set2 = ''
set3 = []
numb = 0
lines = []

for ln in s:
    ln = ln.split()
    ln = ln[1:2+1] #only  the var type and var name are wanted
    set1 = 'properties.valueTypeName[' + str(numb) + '] = typeof(' + ln[0] + ').Name;'
    set2 = 'properties.propertyName[' + str(numb) + '] = "' + ln[1] + '";'
    set3.append('properties.value[' + str(numb) + '] = default(' + ln[0] +').ToString();\n')
    numb += 1
    lines.append(set1 + '\n' + set2 + '\n\n')
final = 'EffectProperties properties = new EffectProperties(GetType().Name,'
final += 'numberOfEffectProperties,forGUI);\n       if(forGUI) {'
final += ''.join(lines)
final += '}\n'
final += ''.join(set3) # values must always be set
final += '\nreturn properties;'
reader.clipboard_clear()
reader.clipboard_append(final)
	*/
}

/// <summary>
/// this structure does not include dynamic properties that 
/// change over runtime =>(locations, actors should not be set here)
/// create with valueTypesOnly set to true for better runtime performance 
/// since value type and name are null
/// </summary>
/*TODO add checksum to insure name and valueTypeName match 
 * the expected values when using setEffectProperties */
[System.Serializable]
public struct EffectProperties {
	public EffectProperties(string effectClassName, int numberOfProperties, bool addAdditonalValueInformation) {
		this.effectClassName = effectClassName;
		if(addAdditonalValueInformation) {
			propertyName = new string[numberOfProperties];
			valueTypeName = new string[numberOfProperties];
		} else {
			propertyName = null;
			valueTypeName = null;
		}

		value = new string[numberOfProperties];

	}
	/// <summary>
	/// the class name of the effect
	/// </summary>
	public string effectClassName;

	///the name of the property variable
	public string[] propertyName;
	///used by the gui to decide what element is used to set this value
	public string[] valueTypeName;
	///values are converted using parse and ToString() methods
	public string[] value;
}


public enum EffectTypes {
	damageDealtChange,
	damageReceavedChange,
	damageOverTime,
	preventCardPlaying,
	modifyProjectileSpeed,
	changeEquipment,
	changeSpeed,
	damageOnContact
}