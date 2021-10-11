using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkerGestureController : GestureController
{
  // Declare a struct to contain the gestures parameters.
  // It is marked as Serializeable so that Unity can display it in its UI.
  [System.Serializable]
  public struct Parameters
  {
    public float duration;
    public float frequency;
  }

  // Instantiate the parameters for the blinker gesture
  public Parameters GestureParameters;

  // When activating a blinker gesture, we send the duration/frequency parameters
  public override object GetActivateParameters()   { return GestureParameters; }
}
