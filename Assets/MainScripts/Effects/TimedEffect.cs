
public abstract class TimedEffect : Effect {

	public TimedEffect(bool requiresCreator, bool mustBeInitalized) : base(requiresCreator, mustBeInitalized) {
	}

	/// <summary>
	/// called by EffectContainor
	/// can be overridden for custom or multiple timed effects
	/// </summary>
	/// <param name="time_seconds">time passed since this was last called</param>
	/// <returns>whether effect should be applied now</returns>
	public abstract bool incrmentTimer(float time_seconds);
}
