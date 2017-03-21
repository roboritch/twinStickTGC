
using UnityEngine.EventSystems;

public interface IGetAim : UnityEngine.EventSystems.IEventSystemHandler {
	/// <summary>
	/// returns the location of the aim object in world space
	/// </summary>
	/// <param name="aimLocation"></param>
	void getAim(out UnityEngine.Vector2 aimLocation);
}
