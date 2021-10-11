using Newtonsoft.Json.Linq;
using Util;

public class WebClientService_Gestures : WebClientService
{
  JSONObject GetCommand(string command, string gestureName, object args)
  {
    JSONObject packet = new JSONObject();
    packet.Add("command").Set(command);
    packet.Add("gesture").Set(gestureName);

    if (args != null)
      packet.Add("args").Set(new JSONObject(args));

    return packet;
  }

  public void ActivateGesture(string name, object args = null)
  {
    Send(GetCommand("activate", name, args));
  }

  public void UpdateGesture(string name, object args = null)
  {
    Send(GetCommand("update", name, args));
  }

  public void DeactivateGesture(string name, object args = null)
  {
    Send(GetCommand("deactivate", name, args));
  }

  public override void HandleMessage(JObject token)
  {
  }
}
