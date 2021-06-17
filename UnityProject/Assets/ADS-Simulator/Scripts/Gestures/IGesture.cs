using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGesture
{
  // Return an array describing what type is expected for each parameter
  System.Type[] GetParameterTypes();

  // Activate the gesture.
  // Optionally specify a time to play the gesture for.
  void Activate(params object[] args);

  // Deactivate the gesture
  void Deactivate();
}
