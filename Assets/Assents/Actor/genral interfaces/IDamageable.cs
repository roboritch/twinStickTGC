/// <summary>
/// implemented on all monobehaver scripts that can take damage
/// damage can include only astetic changes (wall chips, scorch marks)
/// </summary>
public interface IDamageable : UnityEngine.EventSystems.IEventSystemHandler {
	bool takeDamage(float amount, DamageTypes damageType);
	bool blocksDamage(float amount, DamageTypes damageType);
}
