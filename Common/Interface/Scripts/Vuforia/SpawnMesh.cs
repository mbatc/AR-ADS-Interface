using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMesh : MonoBehaviour
{
    public GameObject consoleMesh;
    public GameObject imageTarget;
    private GameObject console;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateConsoleMesh()
    {
        Debug.Log("Detected image");
        console = Instantiate(consoleMesh, imageTarget.transform);
        //console.transform.position = new Vector3(-0.043f, -0.441f, 0.486f);
        console.transform.parent = null;
        console.transform.position = this.transform.position;
        Destroy(imageTarget);
    }

    public void TestDetection()
    {
        Debug.Log("Found image");
    }
}
