using System.IO;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using WebSocketSharp.Server;
using Util;

public class WebServer : MonoBehaviour
{
  // When service container.
  // This is used to expose the server configuration to Unity's UI.
  [System.Serializable]
  public class WebServerService
  {
    // This is the name of the service.
    // You can connect to it using a URL 'some.host.name/Name'
    [SerializeField]
    public string Name;

    // This object is duplicated when a service with the given name is created.
    [SerializeField]
    public Transform ServiceTemplate;
  }

  public delegate void Task();

  // The port to host the server on
  public ushort Port = 8080;
  // A list of services available on the server
  public List<WebServerService> Services;
  // Where to pub newly connected/created services
  public Transform ConnectedServicesParent = null;
  // The WebSocketSharp object that handles the connectivity.
  private WebSocketServer Server = null;
  // Tasks to be executed on the main thread
  public Queue<Task> TaskQueue = new Queue<Task>();

  public void Update()
  {
    // We process any queued tasks in the Update so that the scene
    // can be modified by the WebServerService's.
    //
    // This is needed as Unity only allows the scene to be modified
    // on the main thread.
    while (TaskQueue.Count > 0)
      TaskQueue.Dequeue()();
  }

  void OnDestroy()
  {
    Server.Stop();
  }

  void Start()
  {
    if (ConnectedServicesParent == null)
    { // If the parent is not assigned, create somewhere to store the services
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
  public class WebServiceWrapper : WebSocketBehavior
  {
    private Transform serviceObject   = null; // The game object associated with the service
    private WebService webService     = null; // The script that implements the service
    private WebServer  webServer      = null; // The parent server for this web service
    private bool createServiceFailed  = false;

    public WebServiceWrapper(Transform serviceTemplate, Transform serviceParent, WebServer server)
    {
      Debug.Log("Creating new service");
      webServer = server;

      // Enqueue a task to duplicate the service template on the main thread.
      webServer.TaskQueue.Enqueue(() =>
      {
        try
        {
          // Duplicate the template
          serviceObject = Instantiate(serviceTemplate);
          // Place it under the serviceParent object used by the WebServer
          serviceObject.parent = serviceParent;
          // Cache the WebService component so we don't need to find when sending/receiving messages.
          webService = serviceObject.GetComponent<WebService>();
          webService.SocketBehavior = this;
        }
        catch (System.Exception e)
        {
          Debug.Log("Failed to create the service: " + e.Message);
          // Notify the waiting thread if creating the service failed
          createServiceFailed = true;
        }
      });
    }

    ~WebServiceWrapper()
    {
      // Destroy the service GameObject when this wrapper is destroyed
      Debug.Log("Destroying service");
    }

    public void Send(byte[] data) { base.Send(data); }
    public void Send(string data) { base.Send(data); }
    public void Send(FileInfo file) { base.Send(file); }

    public void SendAsync(byte[] data, System.Action<bool> onComplete) { base.SendAsync(data, onComplete); }
    public void SendAsync(string data, System.Action<bool> onComplete) { base.SendAsync(data, onComplete); }
    public void SendAsync(FileInfo file, System.Action<bool> onComplete) { base.SendAsync(file, onComplete); }
    public void SendAsync(Stream stream, int length, System.Action<bool> onComplete) { base.SendAsync(stream, length, onComplete); }

    protected override void OnOpen()
    {
      // Wait until our web service has been created
      while (webService == null)
      {
        Thread.Sleep(1);

        if (createServiceFailed)
        {
          Debug.Log("Failed to create requested service");
          Send("Failed to create requested service");
          return;
        }
      }

      // Queue the OnOpen event to be called.
      webServer.TaskQueue.Enqueue(() => webService.OnOpen());
    }

    protected override void OnClose(WebSocketSharp.CloseEventArgs e)
    {
      if (webService != null)
      {
        // Queue the OnClose event to be called.
        webServer.TaskQueue.Enqueue(() =>
        {
          webService.OnClose(e);
          Destroy(serviceObject.gameObject);
        });
      }
    }

    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
      // Queue the OnError event to be called.
      if (webService != null)
        webServer.TaskQueue.Enqueue(() => webService.OnError(e.Message, e.Exception));
    }

    protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
    {
      // Queue the OnMessage event to be called.
      if (webService != null)
      {
        if (e.IsText)
        { // Parse the data as a JSON object
          JSONObject json = new JSONObject(e.Data);
          webServer.TaskQueue.Enqueue(() => {
            Debug.Log(e.Data);
            webService.OnMessage(json);
          });
        }
        else if (e.IsBinary)
        { // Process the raw binary data
          webServer.TaskQueue.Enqueue(() => webService.OnData(e.RawData));
        }
      }
    }
  }
}
