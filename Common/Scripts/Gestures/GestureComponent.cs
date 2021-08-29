using UnityEngine;
using Newtonsoft.Json.Linq;
using Util;

public class GestureComponent : MonoBehaviour
{
  // The name of the gesture.
  // This is the name that the web service expects to be used in a message.
  public string GestureName;

  // Activate the gesture.
  // Parameters accepted are determined by the sub-class.
  public virtual void StartGesture(JSONObject args) {}

  // Deactivate the gesture
  public virtual void StopGesture(JSONObject args) {}
}
