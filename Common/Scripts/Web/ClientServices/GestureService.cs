using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureService : WebClientService
{
  public string StartCommand = "start";
  public string StopCommand = "stop";

  string GetStartCommand(params object[] args)
  {
    return StartCommand + " " + string.Join(" ", args);
  }

  string GetStopCommand(params object[] args)
  {
    return StopCommand + " " + string.Join(" ", args);
  }

  public void ActivateGesture(string name, params object[] args)
  {
    Send(GetStartCommand(name, args));
  }

  public void DeactivateGesture(string name, params object[] args)
  {
    Send(GetStopCommand(name, args));
  }

  public override void HandleMessage(WebSocketSharp.MessageEventArgs messageData)
  {
  }
}
