using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Util;

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

  public override void OnMessage(JSONObject packet)
  {
    string command = packet["command"].AsString();
    if (command == "list")
    {
      List<string> names = new List<string>();
      foreach (var srcObj in Commands)
        names.Add(srcObj.Name);
      Send(string.Join(", ", names));
      return;
    }
    else if (command == "call")
    {
      string commandName = packet["name"].AsString();
      foreach (var cmd in Commands)
      {
        if (cmd.Name == commandName)
        {
          cmd.Handler.Invoke();
          Send("Success");
          return;
        }
      }

      Send(string.Format("Could not find command {0}", commandName));
    }
  }
}
