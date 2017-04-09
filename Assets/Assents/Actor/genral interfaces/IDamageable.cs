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
	bool takeDamage(float amount, DamageTypes damageType);
	/// <summary>
	/// return true if the damage of this amound and type was blocked
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="damageType"></param>
	/// <returns></returns>
	bool blocksDamage(float amount, DamageTypes damageType);
}
