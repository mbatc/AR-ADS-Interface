using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ScalarGesture : GestureComponent
{
  [Tooltip("When the gesture is activated/deactivated, this target object will be activated/deactivated ")]
  public Transform Target;
  
  public float CurrentValue;

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

  public virtual void UpdateGesture(JSONObject args)
  {
    CurrentValue = args["value"].AsFloat();
  }
}
