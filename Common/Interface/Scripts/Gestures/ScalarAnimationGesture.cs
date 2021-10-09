using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalarAnimationGesture : ScalarGestureBase
{
  public string   AnimationName;
  public Animator GestureAnimation;

  public override void OnUpdateValue(float value)
  {
    GestureAnimation.speed = 0;
    GestureAnimation.Play(AnimationName, 0, value);
  }
}
