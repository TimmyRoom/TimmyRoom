using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDanceArea : MonoBehaviour
{
    [SerializeField] protected DanceJudgingPoint Judge;

    /// <summary>
    /// 판정 영역의 초기화를 위한 함수.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// 판정 영역이 사용자에 의해 인터렉션되었을 때 호출되는 함수.
    /// </summary>
    public abstract void GetEntered(int type, GameObject areaObject);

    /// <summary>
    /// 사용자가 판정 영역을 벗어났을 때 호출되는 함수.
    /// </summary>
    public abstract void GetExited(int type, GameObject areaObject);
}
