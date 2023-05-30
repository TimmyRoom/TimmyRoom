using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleHitArea : MonoBehaviour
{
    /// <summary>
    /// 악기의 타입 번호. 어떤 라인의 노트와 연결되는지 판단하는데 사용된다.
    /// </summary>
    public int InstrumentType;
    public UnityAction action;

    public void OnTriggerEnter(Collider other)
    {
        action.Invoke();
    }
}
