using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDontDestory : MonoBehaviour
{
    public static MakeDontDestory instance;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
