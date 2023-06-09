using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRCameraSetter : MonoBehaviour
{
    void Start()
    {
        GameObject cam = transform.Find("Camera Offset").Find("Main Camera").gameObject;
        cam.SetActive(false);
        cam.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            SceneRecorder.instance.Capture();
            
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneRecorder.instance.CaptureSelf();
    }
}
