using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DanceTriggerArea : MonoBehaviour
{
    // 판정 영역의 타입 번호
    public int AreaType;
    /// <summary>
    /// 해당 오브젝트의 트리거에 진입했을 때 발생하는 액션.
    /// </summary>
    public UnityAction enterAction;
    /// <summary>
    /// 해당 오브젝트의 트리거를 나갔을 때 발생하는 액션.
    /// </summary>
    public UnityAction exitAction;
    /// <summary>
    /// 머테리얼 교체를 위한 판정 영역 오브젝트 자체의 정보.
    /// </summary>
    public GameObject areaObject;

    private void Start()
    {
        areaObject = gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        enterAction.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        exitAction.Invoke();
    }
}
