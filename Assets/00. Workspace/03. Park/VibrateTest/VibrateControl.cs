using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;

public class VibrateControl : MonoBehaviour
{
    //[SerializeField] InputActionReference leftHapticAction;
    //[SerializeField] InputActionReference rightHapticAction;

    [SerializeField] private ActionBasedController rightController;
    [SerializeField] private ActionBasedController leftController;

    public void RunOneSecondWeakVibrate()
    {
        StartCoroutine(CustomVibrateRight(0.4f, 1.0f));
        StartCoroutine(CustomVibrateLeft(0.4f, 1.0f));
    }

    public void RunOneSecondHardVibrate()
    {
        StartCoroutine(CustomVibrateRight(1.0f, 1.0f));
        StartCoroutine(CustomVibrateLeft(1.0f, 1.0f));
    }

    public IEnumerator CustomVibrateRight(float amplitude, float duration)
    {
        //OpenXRInput.SendHapticImpulse(rightHapticAction, amplitude, duration, UnityEngine.InputSystem.XR.XRController.rightHand);
        rightController.SendHapticImpulse(amplitude, duration);
        yield return null;
    }

    public IEnumerator CustomVibrateLeft(float amplitude, float duration)
    {
        //OpenXRInput.SendHapticImpulse(leftHapticAction, amplitude, duration, UnityEngine.InputSystem.XR.XRController.leftHand);
        leftController.SendHapticImpulse(amplitude, duration);
        yield return null;
    }
}
