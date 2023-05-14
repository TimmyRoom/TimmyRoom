using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUIPage : MonoBehaviour
{
    int count = 0;
    public TextMeshProUGUI text;
    public void UP()
    {
        text.text = (++count).ToString();
    }
}
