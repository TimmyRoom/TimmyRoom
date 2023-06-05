using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanceNote : MonoBehaviour
{
    public int type;
    public int Type { get => type; set => type = value; }

    public Image image;

    private void Awake()
    {
        image = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
    }
}
