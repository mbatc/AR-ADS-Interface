using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapDetectorGestureActivator : GestureActivator
{
  public Leap.Unity.Detector Detector
  {
    get
    {
      if (m_detector == null)
        m_detector = GetComponent<Leap.Unity.Detector>();
      return m_detector;
    }
  }
  private Leap.Unity.Detector m_detector = null; // storage for Detector property

  public override bool IsGestureActive()
  {
    return Detector != null && Detector.IsActive;
  }
}
