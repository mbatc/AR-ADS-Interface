using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;

public class WebService_Gesture : WebService
{
  public void HandleMessage(string message)
  {
    string[] args = message.Split(null);

    string command     = "";
    string gestureName = "";
    ParseParameter(args, 0, ref command);
    ParseParameter(args, 1, ref gestureName);

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
          System.Type[] gestureParamTypes = gesture.GetGestureParameterTypes();
          object[] gestureParams = new object[gestureParamTypes.Length];

          for (int i = 0; i < gestureParams.Length; ++i)
            gestureParams[i] = ParseParameter(args, 2 + i, gestureParamTypes[i], null);

          if (command == "start")
            gesture.StartGesture(gestureParams);
          else if (command == "stop")
            gesture.StopGesture();
        }
      }
    }
  }

  public override void OnMessage(MessageEventArgs args)
  {
    if (!args.IsText)
      return;

    HandleMessage(args.Data);
  }

  void ParseParameter<T>(string[] values, int index, ref T into)
  {
    into = (T)ParseParameter(values, index, typeof(T), into);
  }

  object ParseParameter(string []values, int index, System.Type paramType, object defaultValue)
  {
    if (index < 0 || index >= values.Length)
      return defaultValue;

    try
    {
      if (paramType == typeof(string))
        return new string(values[index].ToCharArray());
      else if (paramType == typeof(float))
        return float.Parse(values[index]);
      else if (paramType == typeof(int))
        return int.Parse(values[index]);
    }
    catch (System.Exception e)
    {
      Debug.Log("Failed to parse gesture parameter: " + e.Message);
    }

    return defaultValue;
  }
}
