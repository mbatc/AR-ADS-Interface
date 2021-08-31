using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureController : MonoBehaviour
{
  [Tooltip("Specify the name of the gesture. This must be the same as a Gesture that has been defined in HoloLens Interface Application.")]
  public string GestureName;
  [Tooltip("How often to send an Update command in Hz. The frequency cannot be higher than the current frame rate. If 0 is specified, no updates are sent.")]
  public float  UpdateFrequency;

  // Store a reference to the ClientGestureService so we don't need to search for it each time it is used.
  static private WebClientService_Gestures s_gestureClientService = null;
  // The state of this gesture
  private bool m_isGestureActive   = false; // Is the gesture active
  private bool m_lastGestureActive = false; // Was the gesture active on the previous frame
  private bool m_sendUpdate        = false; // Should an update be sent on the next update
  private float m_lastUpdateTime = 0.0f;

  // Override these to return custom parameters for a each gesture event
  // See BlinkerGestureController for an example.
  public virtual object GetActivateParameters()   { return null; }
  public virtual object GetDeactivateParameters() { return null; }
  public virtual object GetUpdateParameters()     { return null; }

  // Activate this gesture
  public void Activate()
  {
    m_isGestureActive = true;
  }

  // Deactivate this gesture
  public void Deactivate()
  {
    m_isGestureActive = false;
  }

  // Send an Update command on the next scene update
  public void SendUpdate()
  {
    m_sendUpdate = true;
  }

  WebClientService_Gestures GetGestureService()
  {
    if (s_gestureClientService == null)
    {
      foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
      {
        s_gestureClientService = root.GetComponentInChildren<WebClientService_Gestures>();
        if (s_gestureClientService != null)
          break;
      }
    }
    return s_gestureClientService;
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - m_lastUpdateTime > 1.0f / UpdateFrequency)
      SendUpdate();

    WebClientService_Gestures service = GetGestureService();
    if (service == null)
    {
      Debug.Log(string.Format("Cannot activate the gesture '{0}'. No WebClientService_Gestures exists in the active scene.", GestureName));
      return;
    }

    if (m_isGestureActive && !m_lastGestureActive)
    {
      service.ActivateGesture(GestureName, GetActivateParameters());
      m_lastUpdateTime = Time.time;
    }

    if (m_sendUpdate)
    {
      service.UpdateGesture(GestureName, GetUpdateParameters());
      m_lastUpdateTime = Time.time;
    }

    if (!m_isGestureActive && m_lastGestureActive)
      service.DeactivateGesture(GestureName, GetDeactivateParameters());

    m_lastGestureActive = m_isGestureActive;
  }
}
