using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureActivatorCompound : GestureActivator
{
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
    foreach (var activator in Activators)
      if (!activator.IsGestureActive())
        return false;
    return true;
  }

  void OnEnable() { DiscoverActivators(); }
}
