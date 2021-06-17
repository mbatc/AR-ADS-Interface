using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;

public abstract class WebClientService : MonoBehaviour
{
  public string ServiceName; // The name of the service to connect to

  private WebClient m_client = null;
  private WebSocket ws       = null;
  private Queue<WebSocketSharp.MessageEventArgs> m_messageQueue = new Queue<MessageEventArgs>();

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
    WebSocketSharp.MessageEventArgs message = null;
    lock (m_messageQueue)
      message = m_messageQueue.Dequeue();
    HandleMessage(message);
  }

  public abstract void HandleMessage(WebSocketSharp.MessageEventArgs messageData);

  public void SetClient(WebClient client)
  {
    m_client = client;
  }

  public void Connect()
  {
    ws = new WebSocket(string.Format("ws://{0}:{1}/{2}", m_client.ServerIP, m_client.ServerPort, ServiceName));

    ws.OnMessage += (sender, e) =>
    {
      m_messageQueue.Enqueue(e);
    };

    ws.Connect();
  }

  public void Send(string message)
  {
    ws.Send(message);
  }

  public void Send(byte[] data)
  {
    ws.Send(data);
  }

  public void Send(System.IO.FileInfo file)
  {
    ws.Send(file);
  }
}
