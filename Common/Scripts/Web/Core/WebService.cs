using WebSocketSharp;
using UnityEngine;
using Util;

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
  public virtual void OnMessage(JSONObject packet) { /*No default behaviour*/ }
  
  // Called when a binary message is received
  public virtual void OnData(byte[] bytes) { /*No default behaviour*/ }

  // Called when an error occurs
  public virtual void OnError(string message, System.Exception exception)
  {
    Debug.Log("Exception occurred in a WebService: " + exception);
  }

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