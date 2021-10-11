using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Util;

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

  public override void OnMessage(JSONObject packet)
  {
    string objectName = packet["object"].AsString();
    if (objectName == "list")
    {
      List<string> names = new List<string>();
      foreach (var srcObj in SourceObjects)
        names.Add(srcObj.Name);
      Send(string.Join(", ", names));
      return;
    }

    GameObject newObject = null;
    
    Vector3 objectPos = new Vector3();
    Vector3 objectRot = new Vector3();

    objectPos.x = packet["position"][0].As<float>(0.0f);
    objectPos.y = packet["position"][1].As<float>(0.0f);
    objectPos.z = packet["position"][2].As<float>(0.0f);

    objectRot.x = packet["rotation"][0].As<float>(0.0f);
    objectRot.y = packet["rotation"][1].As<float>(0.0f);
    objectRot.z = packet["rotation"][2].As<float>(0.0f);

    // Find the requested object
    foreach (var kvp in SourceObjects)
    {
      if (kvp.Name != objectName)
        continue;

      newObject = kvp.SourceObject;
      break;
    }

    // Duplicate the object if it was found
    if (newObject != null)
    {
      Instantiate(newObject, objectPos, Quaternion.Euler(objectRot));
      Send("Success");
    }
    else
    {
      Send(string.Format("Unknown object name '{0}' (use -list to list the available objects)", objectName[0]));
    }
  }
}
