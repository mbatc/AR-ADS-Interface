using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkerGesture : GestureComponent
{
  private bool  isActive       = false;
  private float blinkStartTime = 0.0f;
  private float blinkDuration  = 0.0f;
  private float blinkFreq      = 0.0f;

  public float DefaultDuration = 3.0f;
  public float DefaultFrequency = 2.0f;   // 2Hz
  public GameObject Target = null; // The gameobject to 'blink'

  // Update is called once per frame
  void Update()
  {
    if (!isActive)
      return;

    float activeTime = Time.time - blinkStartTime;
    if (activeTime >= blinkDuration)
    {
      StopGesture();
    }
    else
    {
      bool showBlinker = (int)Mathf.Floor(activeTime * blinkFreq * 2) % 2 == 1 ? true : false;
      if (showBlinker != Target.activeSelf)
        Target.SetActive(showBlinker);
    }
  }

  public override void StartGesture(params object[] args)
  {
    float duration = -1; // (float)args[0];
    float frequency = -1; //  (float)args[1];

    blinkStartTime = Time.time;
    blinkFreq      = frequency < 0 ? DefaultFrequency : frequency;
    blinkDuration  = duration < 0 ? DefaultDuration : duration;
    isActive = true;
  }
  public override void StopGesture()
  {
    if (Target != null)
      Target.SetActive(false);
    isActive = false;
  }

  public override System.Type[] GetGestureParameterTypes()
  {
    return new System.Type[]{ typeof(float), typeof(float) };
  }
}
