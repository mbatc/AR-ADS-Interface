using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
  public GestureService Service;
  public string TurnLeftGesture;
  public string TurnRightGesture;
  public string StartGesture;
  public string StopGesture;

  public void beginTurnLeft()
  {
    Service.ActivateGesture(TurnLeftGesture);
  }

  public void endTurnLeft()
  {
    Service.DeactivateGesture(TurnLeftGesture);
  }

  public void beginTurnRight()
  {
    Service.ActivateGesture(TurnRightGesture);
  }

  public void endTurnRight()
  {
    Service.DeactivateGesture(TurnRightGesture);
  }

  public void beginStart()
  {
    Service.ActivateGesture(StartGesture);
  }

  public void endStart()
  {
    Service.DeactivateGesture(StartGesture);
  }

  public void beginStop()
  {
    Service.ActivateGesture(StopGesture);
  }

  public void endStop()
  {
    Service.DeactivateGesture(StopGesture);
  }
}
