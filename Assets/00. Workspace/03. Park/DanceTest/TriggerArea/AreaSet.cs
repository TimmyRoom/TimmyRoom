using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSet : AbstractDanceArea
{
    public DanceTriggerArea[] TriggerAreas;

    public bool[] isTriggered = { false, false, false, false, false, false };

    public ReflectionProbe mirror;

    public Material blue;
    public Material origin;
    public Material transparent;

    public void EnableGuide()
    {
        foreach(var area in TriggerAreas)
        {
            area.areaObject.GetComponent<MeshRenderer>().material = origin;
        }
        mirror.cullingMask |= 1 << LayerMask.NameToLayer("Guide");
    }

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
                isTriggered[1] = false; isTriggered[2] = false;
                isTriggered[0] = true;
                break;
            case 12:
                isTriggered[0] = false; isTriggered[2] = false;
                isTriggered[1] = true;
                break;
            case 13:
                isTriggered[0] = false; isTriggered[1] = false;
                isTriggered[2] = true;
                break;
            case 21:
                isTriggered[4] = false; isTriggered[5] = false;
                isTriggered[3] = true;
                break;
            case 22:
                isTriggered[3] = false; isTriggered[5] = false;
                isTriggered[4] = true;
                break;
            case 23:
                isTriggered[3] = false; isTriggered[4] = false;
                isTriggered[5] = true;
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
