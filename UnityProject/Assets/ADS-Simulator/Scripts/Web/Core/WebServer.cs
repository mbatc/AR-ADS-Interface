using System.IO;
using System.Collections.Generic;
using System.Threading;
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
    public Transform ServiceTemplate;
  }

  public ushort Port = 8080; // The port to host the server on
  public List<WebServerService> Services; // A list of services available on the server

  public Transform ConnectedServicesParent = null;
  private WebSocketServer Server = null; // WebSocketSharp server

  public delegate void Task();

  // Tasks to be executed on the main thread
  public Queue<Task> TaskQueue = new Queue<Task>();

  // Update the component
  public void Update()
  {
    while (TaskQueue.Count > 0)
      TaskQueue.Dequeue()();
  }

  void OnDestroy()
  {
    // Stop the server from running
    Server.Stop();
  }

  // Start is called before the first frame update
  void Start()
  {
    if (ConnectedServicesParent == null)
    {
      ConnectedServicesParent = new GameObject().transform;
      ConnectedServicesParent.name = "Connected Services";
      ConnectedServicesParent.transform.parent = transform;
    }

    // Create a WebSocketServer that is listening on the requested port
    Server = new WebSocketServer(Port);
    Debug.Log(string.Format("Hosting web server on {0}", Port));

    // Start the server
    Server.Start();

    // Add services
    foreach (WebServerService service in Services)
    {
      if (service.ServiceTemplate == null)
        continue; // Silently fail, not really an error

      if (service.ServiceTemplate.GetComponent<WebService>() == null)
      {
        Debug.Log(string.Format("Cannot add WebService {0}. The template GameObject must contain a component that implements the IWebService interface.", service.Name));
        continue;
      }

      // Add the service to the web server
      Debug.Log(string.Format("Adding web service '{0}'", service.Name));

      Server.AddWebSocketService<WebServiceWrapper>("/" + service.Name, () => new WebServiceWrapper(service.ServiceTemplate, ConnectedServicesParent, this));
    }
  }

  void Destroy()
  {
    // When this GameObject is destroyed, stop the server
    Server.Stop();
  }

  // A minimal wrapper of WebSocketBehaviour that forwards events received from WebSocketSharp
  // to a unity game object specified. 
  //
  // It does this by adding tasks to a queue to be processed on the Main thread.
  public class WebServiceWrapper : WebSocketBehavior
  {
    private Transform serviceParent   = null; // Where to place the new service in the scene hierarchy
    private Transform serviceTemplate = null; // The prefab that implements the service
    private Transform serviceObject   = null; // The game object associated with the service
    private WebService webService     = null; // The script that implements the service
    private WebServer  webServer      = null; // The parent server for this web service
    private bool createServiceFailed  = false;
    public WebServiceWrapper(Transform serviceTemplate, Transform serviceParent, WebServer server)
    {
      Debug.Log("Creating new service");
      webServer = server;

      webServer.TaskQueue.Enqueue(() =>
      {
        try
        {
          serviceObject = Instantiate(serviceTemplate);
          serviceObject.parent = serviceParent;

          webService = serviceObject.GetComponent<WebService>();
          webService.SocketBehavior = this;
        }
        catch (System.Exception e)
        {
          createServiceFailed = true;
        }
      });
    }

    ~WebServiceWrapper()
    {
      // Destroy the service GameObject when this wrapper is destroyed
      Debug.Log("Destroying service");

    }

    public void _Send(byte[] data) { base.Send(data); }
    public void _Send(string data) { base.Send(data); }
    public void _Send(FileInfo file) { base.Send(file); }

    public void _SendAsync(byte[] data, System.Action<bool> onComplete) { base.SendAsync(data, onComplete); }
    public void _SendAsync(string data, System.Action<bool> onComplete) { base.SendAsync(data, onComplete); }
    public void _SendAsync(FileInfo file, System.Action<bool> onComplete) { base.SendAsync(file, onComplete); }
    public void _SendAsync(Stream stream, int length, System.Action<bool> onComplete) { base.SendAsync(stream, length, onComplete); }

    protected override void OnOpen()
    {
      while (webService == null) {
        Thread.Sleep(1); // Wait until our webservice has been created
        if (createServiceFailed)
        {
          Debug.Log("Failed to create requested service");
          Send("Failed to create requested service");
          return;
        }
      }
      webServer.TaskQueue.Enqueue(() => webService.OnOpen());
    }

    protected override void OnClose(WebSocketSharp.CloseEventArgs e)
    {
      if (webService != null)
        webServer.TaskQueue.Enqueue(() =>
        {
          webService.OnClose(e);
          Destroy(serviceObject.gameObject);
        });
    }

    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
      if (webService != null)
        webServer.TaskQueue.Enqueue(() => webService.OnError(e));
    }

    protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
    {
      if (webService != null)
        webServer.TaskQueue.Enqueue(() => webService.OnMessage(e));
    }
  }
}
