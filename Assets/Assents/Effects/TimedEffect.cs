
public abstract class TimedEffect : Effect {
	protected float maxTime_seconds;
	protected float timeLeft_seconds;

	public TimedEffect(bool requiresCreator, bool mustBeInitalized) : base(requiresCreator, mustBeInitalized) {
	}

	/// <summary>
	/// called by EffectContainor
	/// can be overridden for custom or multiple timed effects
	/// </summary>
	/// <param name="time_seconds">time passed since this was last called</param>
	/// <returns>whether effect should be applied now</returns>
	public virtual bool incrmentTimer(float time_seconds) {
		if(timeLeft_seconds > 0) {
			timeLeft_seconds -= time_seconds;
			return false;
		} else {
			return true;
		}
	}
}
