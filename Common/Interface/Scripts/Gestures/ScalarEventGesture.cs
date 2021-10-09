using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScalarEventGesture : ScalarGestureBase
{
  [System.Serializable]
  public class ScalarEvent : UnityEvent<float> { }

  ScalarEvent UpdateEvent;

  public override void OnUpdateValue(float value)
  {
    UpdateEvent.Invoke(value);
  }
}
