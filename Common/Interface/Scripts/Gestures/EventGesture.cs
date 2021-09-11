﻿using UnityEngine.Events;
using Util;

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

  public override void ActivateGesture(JSONObject args)
  {
    OnStartGesture.Invoke();
  }

  public override void DeactivateGesture(JSONObject args)
  {
    OnStopGesture.Invoke();
  }
}