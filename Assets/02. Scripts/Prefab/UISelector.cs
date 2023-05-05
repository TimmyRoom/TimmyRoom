using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 사용자가 컨트롤러 Ray를 통해 상호작용하여 씬 내부의 오브젝트들의 Unity Event를 Invoke하는 클래스.
/// </summary>
public class UISelector : MonoBehaviour
{
    public float InteractionTime;
    public UnityEvent[] UIEvents;
    IEnumerator InteractionRoutine;

    /// <summary>
    /// StartCoroutine으로 StartTimer 루틴을 실행하고 InteractionRoutine에 저장한다.
    /// </summary>
    public void GetRayCasted()
    {
        InteractionRoutine = StartTimer();
        StartCoroutine(InteractionRoutine);
    }

    /// <summary>
    /// StopCoroutine으로 기존 루틴을 중단한다.
    /// </summary>
    public void GetRayCastStopped()
    {
        StopCoroutine(InteractionRoutine);
    }

    /// <summary>
    /// class 내 설정된 시간이 지나면 할당된 events들을 Invoke한다.
    /// </summary>
    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(InteractionTime);
		foreach(var UIEvent in UIEvents)
		{
				UIEvent?.Invoke();
		}
    }
}
