using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ChartTest : NoteTest
{
    [Range(0, 1)]
    [SerializeField] float clearRate = 0.6f;
    [SerializeField] List<string> failTexts;
    private int hitCount = 0;
    private int failCount = 0;
    public override void SetCount()
    {
        hitCount++;
    }

    protected override IEnumerator BarCoroutine()
    {
        GameChart chart = NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
        while (count >= 0)
        {
            yield return new WaitForSeconds(480 / chart.BPM);
            float hitRate = (float)hitCount / (hitCount + failCount);
            if(hitRate >= clearRate || count == 0)
            {
                break;
            }
            else
            {
                text.text = failTexts[failTexts.Count - (count--)];
                NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
                hitCount = 0;
                failCount = 0;
            }
        }
        NantaScenarioManager.instance.ResetAll();
        NantaScenarioManager.instance.SetScenario(nextScenario);
    }

    public void MissNote()
    {
        failCount++;
    }

    public override Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 1, MissNote }, { 2, SetCount }, { 3, MissNote }, { 4, StartBar} };
    }
}
