using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 비상탈출구 오브젝트 클래스.
/// </summary>
public class EscapeDoor : MonoBehaviour
{
    /// <summary>
    /// 탈출 버튼 작동 시 재생되는 효과음.
    /// </summary>
    public AudioClip EscapeSound;
    
    /// <summary>
    /// 탈출 버튼 작동 시 실행되는 이벤트.
    /// </summary>
    public UnityEvent EscapeEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LeftController") || other.CompareTag("RightController"))
        {
            StartCoroutine(Escape());
        }
    }

    /// <summary>
    /// 일정 시간을 두고 EscapeEvent를 발생시킨다.
    /// </summary>
    IEnumerator Escape()
    {
        SoundManager.instance.PlaySound(EscapeSound, gameObject.GetComponent<AudioSource>());
        yield return new WaitForSeconds(0.75f);
        EscapeEvent?.Invoke();
    }
}