using UnityEngine;
using Util;

public class ToggleGesture : GestureComponent
{
  [Tooltip("When the gesture is activated/deactivated, this target object will be activated/deactivated ")]
  public Transform Target;

  public override void ActivateGesture(JSONObject args)
  {
    if (Target != null)
      Target.gameObject.SetActive(true);
  }

  public override void DeactivateGesture(JSONObject args)
  {
    if (Target != null)
      Target.gameObject.SetActive(false);
  }
}
