using Util;

public class WebService_Gesture : WebService
{
  public override void OnMessage(JSONObject packet)
  {
    string command     = packet["command"].AsString();
    string gestureName = packet["gesture"].AsString();

    if (command != "")
    {
      if (gestureName != "")
      {
        GestureComponent gesture = null;
        foreach (GestureComponent candidate in transform.GetComponentsInChildren<GestureComponent>())
          if (candidate.GestureName == gestureName)
          {
            gesture = candidate;
            break;
          }

        if (gesture != null)
        {
          if (command == "activate")
            gesture.ActivateGesture(packet["args"]);
          else if (command == "deactivate")
            gesture.DeactivateGesture(packet["args"]);
          else if (command == "update")
            gesture.UpdateGesture(packet["args"]);
        }
      }
    }
  }
}
