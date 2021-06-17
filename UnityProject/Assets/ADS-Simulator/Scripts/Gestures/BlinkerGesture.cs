using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkerGesture
  : MonoBehaviour
  , IGesture
{
  private bool  isActive       = false;
  private float blinkStartTime = 0.0f;
  private float blinkDuration  = 0.0f;
  private float blinkFreq      = 0.0f;

  public float DefaultDuration = 3.0f;
  public float DefaultFrequency = 2.0f;   // 2Hz
  public GameObject Target = null; // The gameobject to 'blink'

  // Start is called before the first frame update
  void Start() { Activate(-1.0f, -1.0f); }

  // Update is called once per frame
  void Update()
  {
    if (!isActive)
      return;


    float activeTime = Time.time - blinkStartTime;
    if (activeTime >= blinkDuration)
    {
      Deactivate();
    }
    else
    {
      bool showBlinker = (int)Mathf.Floor(activeTime * blinkFreq * 2) % 2 == 1 ? true : false;
      if (showBlinker != Target.activeSelf)
        Target.SetActive(showBlinker);
    }
  }

  public void Activate(params object[] args)
  {
    float duration  = (float)args[0];
    float frequency = (float)args[1];

    blinkStartTime = Time.time;
    blinkFreq      = frequency < 0 ? DefaultFrequency : frequency;
    blinkDuration  = duration < 0 ? DefaultDuration : duration;
    isActive = true;
  }
  public void Deactivate()
  {
    if (Target != null)
      Target.SetActive(false);
    isActive = false;
  }

  public System.Type[] GetParameterTypes()
  {
    return new System.Type[]{ typeof(float), typeof(float) };
  }
}
