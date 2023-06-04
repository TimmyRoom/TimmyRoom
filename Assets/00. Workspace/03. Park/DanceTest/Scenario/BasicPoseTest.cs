using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BasicPoseTest : MonoBehaviour, IScenario
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected int count = 100;

    public virtual void Start()
    {
        text.text = count.ToString();
    }

    public virtual void SetCount()
    {
        text.text = (--count).ToString();
        //if (count == 0)
        //{
        //    DanceScenarioManager.instance.SetScenario(2);
        //}
    }

    public virtual Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 1, SetCount } };
    }
}
