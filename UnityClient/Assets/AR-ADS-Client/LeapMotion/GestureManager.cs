using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
  struct BlinkData
  {
    public BlinkData(float dur, float freq)
    {
      duration = dur;
      frequency = freq;
    }

    public float duration;
    public float frequency;
  }

  public GestureService Service;
  public string TurnLeftGesture;
  public string TurnRightGesture;
  public string StartGesture;
  public string StopGesture;

  public void beginTurnLeft()
  {
    Service.ActivateGesture(TurnLeftGesture, new BlinkData(-1, -1));
  }

  public void endTurnLeft()
  {
    Service.DeactivateGesture(TurnLeftGesture, new BlinkData(-1, -1));
  }

  public void beginTurnRight()
  {
    Service.ActivateGesture(TurnRightGesture, new BlinkData(-1, -1));
  }

  public void endTurnRight()
  {
    Service.DeactivateGesture(TurnRightGesture, new BlinkData(-1, -1));
  }

  public void beginStart()
  {
    Service.ActivateGesture(StartGesture, new BlinkData(-1, -1));
  }

  public void endStart()
  {
    Service.DeactivateGesture(StartGesture, new BlinkData(-1, -1));
  }

  public void beginStop()
  {
    Service.ActivateGesture(StopGesture, new BlinkData(-1, -1));
  }

  public void endStop()
  {
    Service.DeactivateGesture(StopGesture, new BlinkData(-1, -1));
  }
}
