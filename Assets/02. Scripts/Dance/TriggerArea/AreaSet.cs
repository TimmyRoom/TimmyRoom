using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 진행에 사용되는 트리거 구역을 관리하는 클래스이다.
/// </summary>
public class AreaSet : AbstractDanceArea
{
    /// <summary>
    /// 각 판정 영역에 대한 정보를 담고 있는 구조체.
    /// </summary>
    public DanceTriggerArea[] TriggerAreas;

    /// <summary>
    /// 각 판정 영역의 활성화 여부를 담고 있는 배열.
    /// 판정 트리거의 개수인 6을 크기로 한다.
    /// </summary>
    public bool[] isTriggered = { false, false, false, false, false, false };

    /// <summary>
    /// 컬링 마스크 적용을 위한 거울 오브젝트.
    /// </summary>
    public ReflectionProbe mirror;

    /// <summary>
    /// 각 판정 영역의 색을 변경하거나 감추는데 사용하는 머테리얼.
    /// </summary>
    public Material blue; // 판정 영역이 강조될 때의 색.
    public Material origin; // 판정 영역의 기본 색.
    public Material transparent; // 판정 영역이 가려질 때의 투명 머테리얼.

    /// <summary>
    /// 각 판정 영역의 가이드를 활성화시킨다.
    /// </summary>
    public void EnableGuide()
    {
        foreach(var area in TriggerAreas)
        {
            area.areaObject.GetComponent<MeshRenderer>().material = origin;
        }
        mirror.cullingMask |= 1 << LayerMask.NameToLayer("Guide");
    }

    /// <summary>
    /// 각 판정 영역의 가이드를 비활성화시킨다.
    /// </summary>
    public void DisableGuide()
    {
        foreach (var area in TriggerAreas)
        {
            area.areaObject.GetComponent<MeshRenderer>().material = transparent;
        }
        mirror.cullingMask = ~(1 << LayerMask.NameToLayer("Guide"));
    }

    public override void GetEntered(int type, GameObject areaObject)
    {
        if(DanceScenarioManager.instance.currentScenarioNum == 1)
            areaObject.GetComponent<MeshRenderer>().material = blue;

        switch (type)
        {
            case 11:
                isTriggered[0] = true;
                isTriggered[1] = false; isTriggered[2] = false;
                break;
            case 12:
                isTriggered[1] = true;
                isTriggered[0] = false; isTriggered[2] = false;
                break;
            case 13:
                isTriggered[2] = true;
                isTriggered[0] = false; isTriggered[1] = false;
                break;
            case 21:
                isTriggered[3] = true;
                isTriggered[4] = false; isTriggered[5] = false;
                break;
            case 22:
                isTriggered[4] = true;
                isTriggered[3] = false; isTriggered[5] = false;
                break;
            case 23:
                isTriggered[5] = true;
                isTriggered[3] = false; isTriggered[4] = false;
                break;
            default:
                break;
        }
        Judge.UsingTypeForScenario();
    }

    public override void GetExited(int type, GameObject areaObject)
    {
        if (DanceScenarioManager.instance.currentScenarioNum == 1)
            areaObject.GetComponent<MeshRenderer>().material = origin;
    }

    public override void Initialize()
    {
        foreach (var triggerArea in TriggerAreas)
        {
            triggerArea.enterAction = () => { GetEntered(triggerArea.AreaType, triggerArea.gameObject); };
            triggerArea.exitAction = () => { GetExited(triggerArea.AreaType, triggerArea.gameObject); };
        }
        Debug.Log("Area Initialized");
        Judge = FindAnyObjectByType<DanceJudgingPoint>();
    }
}
