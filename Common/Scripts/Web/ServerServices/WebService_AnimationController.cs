using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebService_AnimationController : WebService
{
  public List<Animation> Targets;

  public Animation FindTarget(string name)
  {
    foreach (Animation target in Targets)
      if (target.gameObject.name == name)
        return target;
    return null;
  }

  public List<string> GetTargetNames()
  {
    List<string> names = new List<string>();
    foreach (Animation target in Targets)
        names.Add(target.gameObject.name);
    return names;
  }

  public List<string> GetClipNames(Animation anim)
  {
    List<string> names = new List<string>();
    foreach (AnimationState state in anim)
      names.Add(state.name);
    return names;
  }

  public override void OnMessage(MessageEventArgs args)
  {
    string[] parts = args.Data.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

    if (parts.Length == 0)
    {
      Send("Please send a command");
      return;
    }

    Animation anim = null;
    switch (parts[0])
    {
      case "list_targets":
        Send(string.Join(", ", GetTargetNames()));
        break;
      case "list_anims":
        if (parts.Length != 2)
        {
          Send("Play expects 1 argument.");
          return;
        }

        anim = FindTarget(parts[1]);
        if (anim == null)
        {
          Send(string.Format("Target '{0}' does not exist."));
          return;
        }

        Send(string.Join(", ", GetClipNames(anim)));
        break;
      case "play":
        if (parts.Length != 3)
        {
          Send("Play expects 2 arguments");
          return;
        }

        anim = FindTarget(parts[1]);

        if (anim == null)
        {
          Send(string.Format("Target '{0}' does not exist.", parts[1]));
          return;
        }

        anim.Stop();
        if (GetClipNames(anim).Contains(parts[2]))
        {
          anim.Play(parts[2]);
          Send("Success");
        }
        else
        {
          Send(string.Format("The animation clip {0} does not exist.", parts[2]));
        }

        break;
      case "stop":
        if (parts.Length != 2)
        {
          Send("Play expects 1 argument.");
          return;
        }

        anim = FindTarget(parts[1]);

        if (anim == null)
        {
          Send(string.Format("Target '{0}' does not exist."));
          return;
        }

        anim.Stop();
        Send("Success");
        break;
      default:
        Send(string.Format("Unknown command '{0}'", parts[0]));
        break;
    }
  }
}
