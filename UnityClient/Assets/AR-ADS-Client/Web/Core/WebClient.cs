using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebClient : MonoBehaviour
{
  public string ServerIP = "localhost";
  public ushort ServerPort = 8080;

  public List<WebClientService> Services;

  // Start is called before the first frame update
  void Start()
  {
    foreach (WebClientService service in Services)
    {
      service.SetClient(this);
      service.Connect();
    }
  }
}
