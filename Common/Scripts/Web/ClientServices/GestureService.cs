using Newtonsoft.Json.Linq;
using Util;

public class GestureService : WebClientService
{
  public string StartCommand = "start";
  public string StopCommand  = "stop";

  JSONObject GetCommand(string command, string gestureName, object args)
  {
    JSONObject packet = new JSONObject();
    packet.Add("command").Set(command);
    packet.Add("gesture").Set(gestureName);

    if (args != null)
      packet.Add("args").Set(new JSONObject(args));

    return packet;
  }

  JSONObject GetStartCommand(string name, object args)
  {
    return GetCommand(StartCommand, name, args);
  }

  JSONObject GetStopCommand(string name, object args)
  {
    return GetCommand(StopCommand, name, args);
  }

  public void ActivateGesture(string name, object args = null)
  {
    Send(GetStartCommand(name, args));
  }

  public void DeactivateGesture(string name, object args = null)
  {
    Send(GetStopCommand(name, args));
  }

  public override void HandleMessage(JObject token)
  {
  }
}
