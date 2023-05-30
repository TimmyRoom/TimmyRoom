using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HitTest : MonoBehaviour, IScenario
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float count = 7;
    public void Start()
    {
        text.text = count.ToString();
    }
    public void SetCount()
    {
        text.text = (--count).ToString();
        if(count == 0)
        {
            NantaScenarioManager.instance.SetScenario(2);
        }
    }

    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 1, SetCount }, { 3, SetCount } };
    }
}
