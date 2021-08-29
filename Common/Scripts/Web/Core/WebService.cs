using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;
using System.Collections.Generic;

// This is the base class used for defining a WebService.
// The WebServer forwards events from WebSocketSharp to components of this type.
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

  public void Send(byte[] data) { SocketBehavior.Send(data); }
  public void Send(string data) { SocketBehavior.Send(data); }
  public void Send(System.IO.FileInfo file) { SocketBehavior.Send(file); }

  public void SendAsync(byte[] data, System.Action<bool> onComplete) { SocketBehavior.SendAsync(data, onComplete); }
  public void SendAsync(string data, System.Action<bool> onComplete) { SocketBehavior.SendAsync(data, onComplete); }
  public void SendAsync(System.IO.FileInfo file, System.Action<bool> onComplete) { SocketBehavior.SendAsync(file, onComplete); }
  public void SendAsync(System.IO.Stream stream, int length, System.Action<bool> onComplete) { SocketBehavior.SendAsync(stream, length, onComplete); }
}
