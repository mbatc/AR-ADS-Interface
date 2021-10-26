using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureActivatorSequence : GestureActivator
{
  private int m_nextInSequence = 0;
  private bool m_isActive = false;
  private float m_lastActivatedTime = 0.0f;
  private GestureActivator m_previousGesture = null;

  [Tooltip("The maximum time between gestures activated in the sequence (seconds) If the next gesture in the sequence takes longer than this time, the sequence is reset.")]
  public float MaxDelay = 0.5f;
  public bool FindActivatorsInChildren = true;
  public List<GestureActivator> Activators = new List<GestureActivator>();

  // Find all GestureActivator components contained within this activators children
  public void DiscoverActivators()
  {
    // Clear the current list
    if (FindActivatorsInChildren)
    {
      Activators.Clear();
      for (int childIndex = 0; childIndex < transform.childCount; ++childIndex)
      {
        GestureActivator childActivator = transform.GetChild(childIndex).GetComponent<GestureActivator>();
        if (childActivator != null) // If an activator was found, add it to the list
          Activators.Add(childActivator);
      }
    }
  }

  // Define the condition to activate the gesture.
  // This is True if all child activators are True
  public override bool IsGestureActive()
  {
    if (Activators.Count == 0)
      return false;

    if (m_isActive)
    { // Stay active until the final gesture is deactivated
      m_isActive = Activators[Activators.Count - 1].IsGestureActive();
      if (!m_isActive)
        ResetSequence();
    }
    else
    { // Perform logic to activate this gesture
      if (Activators[m_nextInSequence].IsGestureActive())
      {
        Debug.Log(string.Format("Activated Sequence index {0}", m_nextInSequence));
        m_previousGesture = Activators[m_nextInSequence];
        ++m_nextInSequence;
        m_isActive = m_nextInSequence == Activators.Count;

        if (m_isActive)
          Debug.Log("Sequence gesture activated");
      }
      else
      {
        if (m_previousGesture != null && m_previousGesture.IsGestureActive())
          m_lastActivatedTime = Time.time; // If the previous gesture is still active don't reset the sequence

        if (Time.time - m_lastActivatedTime > MaxDelay)
          ResetSequence();
      }
    }

    return m_isActive;
  }

  void ResetSequence()
  {
    m_previousGesture = null;
    m_nextInSequence = 0;
  }

  void OnEnable() { DiscoverActivators(); }
}
