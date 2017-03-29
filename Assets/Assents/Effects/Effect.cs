namespace EffectNS {
public abstract class Effect{
	/// <summary>
	/// used by the effect container to know what effect this is
	/// </summary>
	private readonly EffectTypes effectType;
	public int getEffectType{
		get { return usesLeft; }
	}

		private int usesLeft;
	/// <summary>
	/// read only: once equal to 0 this effect should be removed
	/// must be checked by any effect contaniers whenevner this effects
	/// methods are called
	/// </summary>
	public int NumberOfUsesLeft{
		get{return usesLeft;}
	}

	/// <summary>
	/// apply some effect to the actor (damage, movment changes, ext)
	/// try to get this methed to work first
	/// </summary>
	/// <param name="applyTo"></param>
	public abstract void applyEffect(Actor applyTo);
}

public enum EffectTypes {
	damageDealtChange,
	damageReceavedChange,
	damageOverTime,
	preventCardPlaying,
	changeEquipment,
	changeSpeed
}
}