using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 댄스 포즈 트리거 구역을 담당하는 클래스이다.
/// </summary>
public class DanceAreaManager : MonoBehaviour
{
    /// <summary>
    /// 씬 진행에 사용되는 모든 구역들의 집합.
    /// </summary>
    public AreaSet area;

    /// <summary>
    /// 초기 설정을 위한 함수.
    /// </summary>
    public void Initialize()
    {
        area.Initialize();
    }

    /// <summary>
    /// 씬 시작 상태로 되돌리는 함수.
    /// </summary>
    public void ResetAll()
    {
        area.gameObject.SetActive(false);
    }
}
