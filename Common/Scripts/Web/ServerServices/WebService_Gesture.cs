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
          if (command == "start")
            gesture.StartGesture(packet["args"]);
          else if (command == "stop")
            gesture.StopGesture(packet["args"]);
        }
      }
    }
  }
}
