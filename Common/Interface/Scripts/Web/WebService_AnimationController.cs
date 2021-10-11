using System.Collections.Generic;
using UnityEngine;
using Util;

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

  public override void OnMessage(JSONObject packet)
  {
    string command = packet["command"].AsString();
    string target  = packet["target"].AsString();
    string animID  = packet["animID"].AsString();
    Animation anim = FindTarget(target);

    switch (command)
    {
      case "list_targets":
        {
          Send(string.Join(", ", GetTargetNames()));
        }
        break;

      case "list_anims":
        {
          if (anim == null)
          {
            Send(string.Format("Target '{0}' does not exist."));
            return;
          }

          Send(string.Join(", ", GetClipNames(anim)));
        }
        break;

      case "play":
        {
          if (anim == null)
          {
            Send(string.Format("Target '{0}' does not exist.", target));
            return;
          }

          anim.Stop();
          if (GetClipNames(anim).Contains(animID))
          {
            anim.Play(animID);
            Send("Success");
          }
          else
          {
            Send(string.Format("The animation clip {0} does not exist.", animID));
          }
        }
        break;

      case "stop":
        {
          if (anim == null)
          {
            Send(string.Format("Target '{0}' does not exist."));
            return;
          }

          anim.Stop();
          Send("Success");
        }
        break;

      default:
        {
          Send(string.Format("Unknown command '{0}'", command));
        }
        break;
    }
  }
}
