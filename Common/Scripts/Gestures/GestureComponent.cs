using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureComponent : MonoBehaviour
{
  // The name of the gesture.
  // This is the name that the web service expects to be used in a message.
  public string GestureName;

  // Return an array describing what type is expected for each parameter
  public virtual System.Type[] GetGestureParameterTypes() { return new System.Type[0]; }

  // Activate the gesture.
  // Parameters accepted are determined by the sub-class.
  public virtual void StartGesture(params object[] args) {}

  // Deactivate the gesture
  public virtual void StopGesture() {}
}
