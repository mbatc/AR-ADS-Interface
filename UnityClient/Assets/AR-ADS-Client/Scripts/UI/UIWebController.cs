using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWebController : MonoBehaviour
{
  public WebClient      WebClient;
  public UnityEngine.UI.InputField BlinkDurationInput;
  public UnityEngine.UI.InputField BlinkFrequencyInput;

  public GestureService Gestures
  {
    get
    {
      if (_gestures == null && WebClient != null)
        _gestures = WebClient.GetComponentInChildren<GestureService>();
      return _gestures;
    }
  }
  private GestureService _gestures;

  public void ActivateBlinker()
  {
    Gestures.ActivateBlinker(float.Parse(BlinkDurationInput.text), float.Parse(BlinkFrequencyInput.text));
  }

  public void DeactivateBlinker()
  {
    Gestures.DeactivateBlinker();
  }
}
