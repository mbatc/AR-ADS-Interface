using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalarGestureController : GestureController
{
  public ScalarValueProducer ValueProducer;

  ScalarValueProducer Producer
  {
    get
    {
      if (ValueProducer == null)
        ValueProducer = GetComponent<ScalarValueProducer>();
      return ValueProducer;
    }
  }

  // Declare a struct to contain the gestures parameters.
  // It is marked as Serializeable so that Unity can display it in its UI.
  [System.Serializable]
  public struct Parameters
  {
    public float value;
  }

  // When activating a blinker gesture, we send the duration/frequency parameters
  public override object GetUpdateParameters()
  {
    ScalarValueProducer producer = Producer;
    Parameters param;
    param.value = producer == null ? 0 : producer.GetScalarValue();
    return param;
  }
}
