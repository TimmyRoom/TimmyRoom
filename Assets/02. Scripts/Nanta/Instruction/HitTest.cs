using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HitTest : MonoBehaviour, IInstruction
{
    public TextMeshProUGUI text;
    private float count = 0;

    public void SetCount()
    {
        text.text = (++count).ToString();
        if(count == 4)
        {
            NantaScenarioManager.instance.SetScenario(5);
        }
    }

    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 1, SetCount }, { 3, SetCount } };
    }
}
