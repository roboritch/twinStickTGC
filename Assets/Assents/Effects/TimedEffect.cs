
public abstract class TimedEffect : Effect {
	public float timeLeft_seconds;
	/// <summary>
	/// called by EffectContainor
	/// </summary>
	/// <param name="time_seconds"></param>
	/// <returns>wether effect should be applyed now</returns>
	public abstract bool incrmentTimer(float time_seconds);
}
