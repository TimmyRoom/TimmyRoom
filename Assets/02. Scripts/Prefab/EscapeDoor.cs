using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 비상탈출구 오브젝트 클래스.
/// </summary>
public class EscapeDoor : MonoBehaviour
{
    public AudioClip EscapeSound;
    public UnityEvent EscapeEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LeftController") || other.CompareTag("RightController"))
        {
            StartCoroutine(Escape());
        }
    }

    IEnumerator Escape()
    {
        SoundManager.instance.PlaySound(EscapeSound, gameObject.GetComponent<AudioSource>());
        yield return new WaitForSeconds(0.75f);
        EscapeEvent?.Invoke();
    }
}