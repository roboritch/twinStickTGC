using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DT {
	public enum DamageTypes {
		/// <summary>
		/// bast waves and large gusts of wind
		/// </summary>
		phyisical_diffuse,
		/// <summary>
		/// normal punches, thrown objects, blunt physical weapons
		/// </summary> 
		physical_normal,
		/// <summary>
		/// fast moving bullets, tip of a speer, slash of a sword. 
		/// </summary>
		phyisical_pearcing,
		/// <summary>
		/// acids
		/// </summary>
		chemical,
		radiation,
		/// <summary>
		/// fire, heat waves, weapon overheats
		/// </summary>
		heat,
		/// <summary>
		/// NOT ice hitting the enamy, extream cold causeing damage
		/// </summary>
		cold,
		elelectric,
		/// <summary>
		/// mind blast ext
		/// </summary>
		psychic,
		/// <summary>
		/// Pure magic
		/// </summary>
		arcanine
	}
}
