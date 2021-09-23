using UnityEngine.Events;
using Util;

[System.Serializable]
public class GestureEvent : UnityEvent<JSONObject> {}

public class EventGesture : GestureComponent
{
  public GestureEvent OnStartGesture;
  public GestureEvent OnStopGesture;
  public GestureEvent OnUpdateGesture;

  public override void ActivateGesture(JSONObject args)
  {
    OnStartGesture.Invoke(args);
  }

  public override void DeactivateGesture(JSONObject args)
  {
    OnStopGesture.Invoke(args);
  }

  public override void UpdateGesture(JSONObject args)
  {
    OnStopGesture.Invoke(args);
  }
}
