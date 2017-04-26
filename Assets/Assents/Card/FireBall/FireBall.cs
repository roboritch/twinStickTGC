using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : ProjectileWeaponBase {
	#region Initialization of static members
	static FireBall() { } //insures these values are overwritten properly
	public new static readonly bool removeOnDraw = true;
	public new static readonly float probabiltyOfDraw = 1f;
	#endregion

	#region initialization of parent vars
	// sprite is done via the unity inspector by 
	// clicking on this script in the project assets window

	public FireBall() {
		cardReloadTime_seconds = 5f;
		cardResorceCost = 1f;
		cardArt = CardPrefabResorceLoader.Instance.loadSprite(getIconPath());
	}

	#endregion



}
