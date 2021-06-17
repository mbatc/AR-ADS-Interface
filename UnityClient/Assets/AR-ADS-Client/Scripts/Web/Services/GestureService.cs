using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureService : WebClientService
{
  public string StartCommand = "start";
  public string StopCommand = "stop";

  public string BlinkName = "Blink_Indicator";

  string GetStartCommand(params object[] args)
  {
    return StartCommand + " " + string.Join(" ", args);
  }

  string GetStopCommand(params object[] args)
  {
    return StopCommand + " " + string.Join(" ", args);
  }

  public void ActivateBlinker(float duration = -1, float frequency = -1)
  {
    Send(GetStartCommand(BlinkName, duration, frequency));
  }

  public void DeactivateBlinker()
  {
    Send(GetStopCommand(BlinkName));
  }

  public override void HandleMessage(WebSocketSharp.MessageEventArgs messageData)
  {
  }
}
