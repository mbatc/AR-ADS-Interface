using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalarTransformGesture : ScalarGestureBase
{
  // The target object for the gesture to influence
  public Transform Target;
  // Update the objects local transform
  public bool ApplyTranslation = true;
  // Update the objects local transform
  public bool ApplyRotation = true;
  // Update the objects local transform
  public bool ApplyScale = true;

  // The minimum scalar value
  public float MinimumValue = 0;
  // The maximum scalar value
  public float MaximumValue = 1;

  // The target object for the gesture to influence
  public Transform StartTransform;
  // The target object for the gesture to influence
  public Transform EndTransform;

  public override void OnUpdateValue(float value)
  {
    float t = (value - MinimumValue) / (MaximumValue - MinimumValue);
    if (ApplyTranslation)
      Target.transform.position = Vector3.Lerp(StartTransform.position, EndTransform.position, t);

    if (ApplyRotation)
    {
      Vector3 rot = StartTransform.eulerAngles;
      for (int axis = 0; axis < 3; ++axis)
        rot[axis] = Mathf.LerpAngle(rot[axis], EndTransform.eulerAngles[axis], t);
      Target.transform.eulerAngles = rot;
    }

    if (ApplyScale)
      Target.transform.localScale = Vector3.Lerp(StartTransform.localScale, EndTransform.localScale, t);
  }
}
