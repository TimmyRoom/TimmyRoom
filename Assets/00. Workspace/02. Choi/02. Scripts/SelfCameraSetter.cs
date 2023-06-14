using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfCameraSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneRecorder.instance.selfCameraObject = this.gameObject;
        this.GetComponent<Camera>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
