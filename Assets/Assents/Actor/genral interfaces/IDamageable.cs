/// <summary>
/// implemented on all monobehaver scripts that can take damage
/// damage can include only astetic changes (wall chips, scorch marks)
/// </summary>
public interface IDamageable : UnityEngine.EventSystems.IEventSystemHandler {
	/// <summary>
	/// return false if no damage is taken
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="damageType"></param>
	/// <returns></returns>
	bool takeDamage(float amount, DamageTypes damageType, DamageSources damageSorce);
	/// <summary>
	/// return true if the damage of this amount and type was blocked from piercing this target 
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="damageType"></param>
	/// <returns></returns>
	bool blocksDamage(float amount, DamageTypes damageType);

	/// <summary>
	/// returns true if damaging object to consider the object hit nonexistent
	/// </summary>
	/// <param name="Sorce"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	bool ignoreDamage(DamageSources damageSorce, DamageTypes damageType);
}

[System.Flags]
public enum DamageSources {
	none = 0,
	player1 = 1 << 0,
	player2 = 1 << 1,
	player3 = 1 << 2,
	player4 = 1 << 3,
	AIGroup1 = 1 << 4,
	AIGroup2 = 1 << 5,
	AIGroup3 = 1 << 6,
}
