using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventGesture : GestureComponent
{
  public UnityEvent OnStartGesture;
  public UnityEvent OnStopGesture;

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  public override System.Type[] GetGestureParameterTypes()
  {
    return new System.Type[0];
  }

  public override void StartGesture(params object[] args)
  {
    OnStartGesture.Invoke();
  }

  public override void StopGesture()
  {
    OnStopGesture.Invoke();
  }
}
