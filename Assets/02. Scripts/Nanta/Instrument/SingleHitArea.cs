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
    /// 중복 판정을 피하기 위해 발생하는 딜레이 구간인지 판별하는 변수.
    /// </summary>
    bool isSleeping = false;
    /// <summary>
    /// 해당 오브젝트의 트리거에 진입했을 때 발생하는 액션.
    /// </summary>
    public UnityAction action;
    public void OnTriggerEnter(Collider other)
    {
        if(isSleeping)
        {
            return;
        }
        action.Invoke();
    }
    /// <summary>
    /// 중복 판정을 피하기 위해 딜레이를 발생시킨다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Sleep()
    {
        isSleeping = true;
        yield return new WaitForSeconds(0.1f);
        isSleeping = false;
    }
}
