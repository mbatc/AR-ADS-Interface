using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;

public class WebService_AddObject : WebService
{
  [System.Serializable]
  public class SrcObjectDef
  {
    [SerializeField]
    public string Name;

    [SerializeField]
    public GameObject SourceObject;
  }

  // Would like to use a dictionary here, but unity can't serialize it
  public List<SrcObjectDef> SourceObjects;

  public override void OnMessage(MessageEventArgs args)
  {
    if (!args.IsText)
    {
      Send("Bad Data");
      return; // This service only parses text commands
    }

    if (args.Data == "-list")
    {
      List<string> names = new List<string>();
      foreach (var srcObj in SourceObjects)
        names.Add(srcObj.Name);
      Send(string.Join(", ", names));
      return;
    }

    string[] command = args.Data.Split();
    if (command.Length < 4)
    {
      Send("Not enough arguments. Please specify 'name' 'x' 'y' 'z'");
      return;
    }

    GameObject newObject = null;
    Vector3 pos = new Vector3(0, 0, 0);

    // Find the requested object
    foreach (var kvp in SourceObjects)
    {
      if (kvp.Name != command[0])
        continue;

      newObject = kvp.SourceObject;
      float.TryParse(command[1], out pos.x);
      float.TryParse(command[2], out pos.y);
      float.TryParse(command[3], out pos.z);
      break;
    }

    // Duplicate the object if it was found
    if (newObject != null)
    {
      Instantiate(newObject, pos, Quaternion.identity);
      Send("Success");
    }
    else
    {
      Send(string.Format("Unknown object name '{0}' (use -list to list the available objects)", command[0]));
    }
  }
}
