using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    public AnimationClip idle;
    public AnimationClip die;
    public GameObject deer1;
    public GameObject deer2;

    public void fist()
    {
        Debug.Log("Fist registered");

        deer1.GetComponent<Animation>().clip = idle;
        deer2.GetComponent<Animation>().clip = die;

        deer1.GetComponent<Animation>().Play();
        deer2.GetComponent<Animation>().Play();
    }

    public void ok()
    {
        Debug.Log("Okay registered");

        deer1.GetComponent<Animation>().clip = die;
        deer2.GetComponent<Animation>().clip = idle;

        deer1.GetComponent<Animation>().Play();
        deer2.GetComponent<Animation>().Play();
    }

    public void palm()
    {
        Debug.Log("Palm registered");

        deer1.GetComponent<Animation>().Stop();
        deer2.GetComponent<Animation>().Stop();
    }
}
