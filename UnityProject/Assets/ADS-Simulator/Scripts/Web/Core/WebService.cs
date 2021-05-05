using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;
using System.Collections.Generic;

public abstract class WebService : MonoBehaviour
{
  public WebServer.WebServiceWrapper SocketBehavior = null;

  // Called when a new connection is opened with this service
  public virtual void OnOpen()
  {
    Debug.Log("New Service Connected");
  }

  // Called when a message is received
  public virtual void OnMessage(MessageEventArgs args) {}

  // Called when an error occurs
  public virtual void OnError(ErrorEventArgs args) {}

  // Called when the connection is closed
  public virtual void OnClose(CloseEventArgs args)
  {
    Debug.Log("Service Closed");
  }

  public void Send(byte[] data) { SocketBehavior._Send(data); }
  public void Send(string data) { SocketBehavior._Send(data); }
  public void Send(System.IO.FileInfo file) { SocketBehavior._Send(file); }

  public void SendAsync(byte[] data, System.Action<bool> onComplete) { SocketBehavior._SendAsync(data, onComplete); }
  public void SendAsync(string data, System.Action<bool> onComplete) { SocketBehavior._SendAsync(data, onComplete); }
  public void SendAsync(System.IO.FileInfo file, System.Action<bool> onComplete) { SocketBehavior._SendAsync(file, onComplete); }
  public void SendAsync(System.IO.Stream stream, int length, System.Action<bool> onComplete) { SocketBehavior._SendAsync(stream, length, onComplete); }
}
