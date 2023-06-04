using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceAreaManager : MonoBehaviour
{
    public AreaSet area;

    public void Initialize()
    {
        area.Initialize();
    }

    public void ResetAll()
    {
        area.gameObject.SetActive(false);
    }
}
