using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebClient : MonoBehaviour
{
  public string Host = "localhost";
  public ushort Port = 8080;

  WebSocket ws = null;

  // Start is called before the first frame update
  void Start()
  {
    ws = new WebSocket(string.Format("ws://{0}:{1}", Host, Port));
    ws.OnMessage += (sender, e) => {
      Debug.Log(string.Format("Received message from {0}, Data: {1}", ((WebSocket)sender).Url, e.Data));
    };

    ws.Connect();
  }

  void OnDestroy()
  {
    ws.Close();
  }

  // Update is called once per frame
  void Update()
  {
    if (ws == null)
      return;

    if (Input.GetKeyDown(KeyCode.Space))
      ws.Send("Hello");
  }
}
