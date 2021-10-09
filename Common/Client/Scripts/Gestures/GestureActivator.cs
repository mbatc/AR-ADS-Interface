using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureActivator : MonoBehaviour
{
  // Get the gesture controller that is attached to the same GameObject
  public GestureController Controller
  {
    get
    {
      if (m_controller == null)
        m_controller = GetComponent<GestureController>();
      return m_controller;
    }
  }
  private GestureController m_controller = null; // storage for Controller property

  private bool m_disable = false; // Is this controller activator disabled. If true, the Controller will not be updated

  // Override this function to signal when a gesture is activated
  public virtual bool IsGestureActive() { return false; }
  // Force the activator to re-enable. If a GestureController is not available on the
  // next call to Update, the activator will be disabled again.
  public void ReEnable() { m_disable = false; }

  public bool IsDisabled() { return m_disable; }
}
