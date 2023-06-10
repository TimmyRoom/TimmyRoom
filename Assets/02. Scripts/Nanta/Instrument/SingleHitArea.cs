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
    /// <summary>
    /// 해당 오브젝트의 트리거에 진입했을 때 발생하는 액션.
    /// </summary>
    public UnityAction action;
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.transform.position} / {this.transform.position} / {Time.time}");
        if(other.transform.position.y > this.transform.position.y + 0.5f)
        action.Invoke();
    }
}
