
namespace EffectNS {
	public abstract class TimedAndTriggeredEffect : TimedEffect, ICheckEffect {
		/// <summary>
		/// take some input and provide an output based on what the effect is
		/// returnType and inputValue should be specifyed by any child methods
		/// 
		/// this should be used in any case where the actor is checking 
		/// for applicable effects rather than the effect being applyed to the actor
		/// </summary>
		/// <typeparam name="returnType"></typeparam>
		/// <typeparam name="inputValue"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public abstract returnType checkEffectTrigger<returnType, inputValue>(inputValue value);
	}

	public interface ICheckEffect {
		returnType checkEffectTrigger<returnType, inputValue>(inputValue value);
	}
}
