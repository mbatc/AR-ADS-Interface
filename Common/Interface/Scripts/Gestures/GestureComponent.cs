using UnityEngine;
using Newtonsoft.Json.Linq;
using Util;

public class GestureComponent : MonoBehaviour
{
  // The name of the gesture.
  // This is the name that the web service expects to be used in a message.
  [Tooltip("The name of the gesture. This is used by the server to identify the Gesture so it must be unique.")]
  public string GestureName;

  // Activate the gesture.
  // Parameters accepted are determined by the sub-class.
  public virtual void ActivateGesture(JSONObject args) {}

  // Deactivate the gesture
  public virtual void DeactivateGesture(JSONObject args) {}

  // Deactivate the gesture
  public virtual void UpdateGesture(JSONObject args) { }
}
