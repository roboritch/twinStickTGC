using System;

[Flags]
public enum DamageTypes {
	/// <summary>
	/// bast waves and large gusts of wind
	/// </summary>
	phyisical_diffuse = 1,
	/// <summary>
	/// normal punches, thrown objects, blunt physical weapons
	/// </summary> 
	physical_normal = 2,
	/// <summary>
	/// fast moving bullets, tip of a speer, slash of a sword. 
	/// </summary>
	phyisical_pearcing = 4,
	/// <summary>
	/// acids
	/// </summary>
	chemical = 8,
	radiation = 16,
	/// <summary>
	/// fire, heat waves, weapon overheats
	/// </summary>
	heat = 32,
	/// <summary>
	/// NOT ice hitting the enemy, it is extreme cold causing damage
	/// </summary>
	cold = 64,
	elelectric = 128,
	/// <summary>
	/// mind blast ext
	/// </summary>
	psychic = 256,
	/// <summary>
	/// Pure magic
	/// </summary>
	arcanine = 512,
	nullElm = 512*2
}