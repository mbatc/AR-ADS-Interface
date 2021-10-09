using UnityEngine;
using Newtonsoft.Json.Linq;
using Util;

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
      DeactivateGesture(JSONObject.Null);
    }
    else
    {
      bool showBlinker = (int)Mathf.Floor(activeTime * blinkFreq * 2) % 2 == 1 ? true : false;
      if (showBlinker != Target.activeSelf)
        Target.SetActive(showBlinker);
    }
  }

  public override void ActivateGesture(JSONObject args)
  {
    float duration  = args["duration"].As<float>(-1);
    float frequency = args["frequency"].As<float>(-1);

    blinkStartTime = Time.time;
    blinkFreq      = frequency < 0 ? DefaultFrequency : frequency;
    blinkDuration  = duration < 0 ? DefaultDuration : duration;
    isActive = true;
  }

  public override void DeactivateGesture(JSONObject args)
  {
    if (Target != null)
      Target.SetActive(false);
    isActive = false;
  }
}
