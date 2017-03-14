
using UnityEngine.EventSystems;

public interface IGetAim : UnityEngine.EventSystems.IEventSystemHandler {
	void getAim(out UnityEngine.Vector2 aimLocation);
}
