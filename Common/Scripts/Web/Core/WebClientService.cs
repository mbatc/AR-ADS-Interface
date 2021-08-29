using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Util;

public abstract class WebClientService : MonoBehaviour
{
  public string ServiceName; // The name of the service to connect to
  public bool IsConnected    = false;
  private WebClient m_client = null;
  private WebSocket ws       = null;
  private Queue<MessageEventArgs> m_messageQueue = new Queue<MessageEventArgs>();

  void Update()
  {
    while (IsMessageAvailable())
      HandleNextMessage();
  }

  private bool IsMessageAvailable()
  {
    int messageCount = 0;

    lock (m_messageQueue)
      messageCount = m_messageQueue.Count;

    return messageCount > 0;
  }

  private void HandleNextMessage()
  {
    MessageEventArgs message = null;
    
    lock (m_messageQueue)
      message = m_messageQueue.Dequeue();

    HandleMessage(JObject.Parse(message.Data));
  }

  public abstract void HandleMessage(JObject messageData);

  public void SetClient(WebClient client)
  {
    m_client = client;
  }

  public void Connect()
  {
    string url = string.Format("ws://{0}:{1}/{2}", m_client.ServerIP, m_client.ServerPort, ServiceName);
    Debug.Log("Connecting to " + url);
    ws = new WebSocket(url);

    ws.OnMessage += (sender, e) =>
    {
      m_messageQueue.Enqueue(e);
    };

    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("Opened " + url);
    };

    ws.OnClose += (sender, e) =>
    {
      Debug.Log("Closed " + url);
    };

    ws.OnError += (sender, e) =>
    {
      Debug.Log("Error " + url);
    };

    ws.Connect();
    IsConnected = true;
  }

  public void Send(JSONObject message)
  {
    if (IsConnected)
      ws.Send(message.ToJSON());
  }

  public void Send(byte[] data)
  {
    if (IsConnected)
      ws.Send(data);
  }

  public void Send(System.IO.FileInfo file)
  {
    if (IsConnected)
      ws.Send(file);
  }
}
