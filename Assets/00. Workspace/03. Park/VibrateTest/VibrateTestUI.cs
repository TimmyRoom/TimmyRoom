using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateTestUI : MonoBehaviour
{
    public void RunOneSecondWeakVibrate()
    {
        StartCoroutine(VibrateControl.instance.CustomVibrateRight(0.4f, 1.0f));
        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(0.4f, 1.0f));
    }

    public void RunOneSecondHardVibrate()
    {
        StartCoroutine(VibrateControl.instance.CustomVibrateRight(1.0f, 1.0f));
        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(1.0f, 1.0f));
    }
}
