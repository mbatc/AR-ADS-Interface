using System.IO;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebServer : MonoBehaviour
{
  [System.Serializable]
  public class WebServerService
  {
    [SerializeField]
    public string Name;

    [SerializeField]
    public GameObject ServiceTemplate;
  }

  public ushort Port = 8080; // The port to host the server on
  public List<WebServerService> Services; // A list of services available on the server

  private WebSocketServer Server = null; // WebSocketSharp server

  // Start is called before the first frame update
  void Start()
  {
    // Create a WebSocketServer that is listening on the requested port
    Server = new WebSocketServer(Port);

    foreach (WebServerService service in Services)
    {
      if (service.ServiceTemplate == null)
        continue; // Silently fail, not really an error

      if (service.ServiceTemplate.GetComponent<WebService>() == null)
      {
        Debug.Log(string.Format("Cannot add WebService {}. The template GameObject must contain a component that implements the IWebService interface.", service.Name));
        continue;
      }

      // Add the service to the web server
      Server.AddWebSocketService("/" + service.Name, () => new WebServiceWrapper(service.ServiceTemplate));
    }

    // Start the server
    Server.Start();
  }

  void Destroy()
  {
    // When this GameObject is destroyed, stop the server
    Server.Stop();
  }

  // A minimal wrapper of WebSocketBehaviour that forwards events received from WebSocketSharp
  // to a unity game object specified. 
  public class WebServiceWrapper : WebSocketBehavior
  {
    private GameObject serviceObject = null; // The gameobject associated with the service
    private WebService webService = null;    // The script that implements the service

    public WebServiceWrapper(GameObject serviceTemplate)
    {
      serviceObject = Instantiate(serviceTemplate);
      webService = serviceObject.GetComponent<WebService>();
      webService.SocketBehavior = this;
    }

    ~WebServiceWrapper()
    {
      // Destroy the service GameObject when this wrapper is destroyed
      Destroy(serviceObject);
    }

    public new void Send(byte[] data)   { base.Send(data); }
    public new void Send(string data)   { base.Send(data); }
    public new void Send(FileInfo file) { base.Send(file); }

    public new void SendAsync(byte[] data, System.Action<bool> onComplete)   { base.SendAsync(data, onComplete); }
    public new void SendAsync(string data, System.Action<bool> onComplete)   { base.SendAsync(data, onComplete); }
    public new void SendAsync(FileInfo file, System.Action<bool> onComplete) { base.SendAsync(file, onComplete); }
    public new void SendAsync(Stream stream, int length, System.Action<bool> onComplete) { base.SendAsync(stream, length, onComplete); }

    protected override void OnOpen()                                     { webService.OnOpen(); }
    protected override void OnClose(WebSocketSharp.CloseEventArgs e)     { webService.OnClose(e); }
    protected override void OnError(WebSocketSharp.ErrorEventArgs e)     { webService.OnError(e); }
    protected override void OnMessage(WebSocketSharp.MessageEventArgs e) { webService.OnMessage(e); }
  }
}
