using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using WebSocketSharp;

class WebService_CommandSet : WebService
{
  [Serializable]
  public struct Command
  {
    // The name of the command
    public string Name;

    // The UnityEvent to handle the command
    public UnityEvent Handler;
  }

  public List<Command> Commands;

  public override void OnMessage(MessageEventArgs args)
  {
    if (!args.IsText)
    {
      Send("Bad Data");
      return;
    }

    if (args.Data == "-list")
    {
      List<string> names = new List<string>();
      foreach (var srcObj in Commands)
        names.Add(srcObj.Name);
      Send(string.Join(", ", names));
      return;
    }

    foreach (var cmd in Commands)
    {
      if (cmd.Name == args.Data)
      {
        cmd.Handler.Invoke();
        Send("Success");
        return;
      }
    }

    Send(string.Format("Could not find command {0}", args.Data));
  }
}
