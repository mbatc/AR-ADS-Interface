using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public abstract class ScalarGestureBase : GestureComponent
{
  // Deactivate the gesture
  public override void UpdateGesture(JSONObject args)
  {
    OnUpdateValue(args["value"].AsFloat());
  }

  // You must override this function to implement a scalar gesture.
  public abstract void OnUpdateValue(float value);
}
