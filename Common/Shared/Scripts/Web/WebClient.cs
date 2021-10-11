using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebClient : MonoBehaviour
{
  public string ServerIP = "localhost";
  public ushort ServerPort = 8080;

  // Start is called before the first frame update
  void Start()
  {
    foreach (WebClientService service in transform.GetComponentsInChildren<WebClientService>())
    {
      Debug.Log("Connecting Service " + service.ServiceName);
      service.SetClient(this);
      service.Connect();
    }
  }
}
