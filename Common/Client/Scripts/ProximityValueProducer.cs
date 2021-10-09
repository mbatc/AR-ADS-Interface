using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityValueProducer
  : MonoBehaviour
  , ScalarValueProducer
{
  public float UpperValue = 1;
  public float LowerValue = 0;

  public Transform InputTransform;
  public Transform UpperTransform;
  public Transform LowerTransform;

  public float GetScalarValue()
  {
    Vector3 maxPos = UpperTransform.position;
    Vector3 minPos = LowerTransform.position;
    Vector3 input  = InputTransform.position;

    Vector3 inputDir = (maxPos - minPos).normalized;
    float   inputMag = (maxPos - minPos).magnitude;
    Vector3 toInput  = input - minPos;
    float   inAmount = Vector3.Dot(inputDir, toInput);
    float t = Mathf.Clamp(inAmount / inputMag, 0, 1);

    Debug.Log(string.Format("In: {0}, Range: {1}, T: {2}", inAmount, inputMag, t));

    Debug.Log(t);

    return LowerValue + t * (UpperValue - LowerValue);
  }
}
