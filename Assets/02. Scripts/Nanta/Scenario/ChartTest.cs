using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ChartTest : NoteTest
{
    public TextMeshProUGUI CountText;
    [Range(0, 1)]
    [SerializeField] float clearRate = 0.6f;
    [SerializeField] List<string> failTexts;
    private int hitCount = 0;
    private int failCount = 0;
    protected override void Start()
    {
        CountText.text = $"{hitCount}/{hitCount + failCount}";
    }
    public override void SetCount()
    {
        hitCount++;
        CountText.text = $"{hitCount}/{hitCount + failCount}";
    }

    protected override IEnumerator MusicCoroutine()
    {
        yield return new WaitForSeconds(NantaScenarioManager.instance.GetWaitTime());
        float hitRate = (float)hitCount / (hitCount + failCount);
        if(hitRate < clearRate && count > 0)
        {
            text.text = failTexts[failTexts.Count - (count--)];
            hitCount = 0;
            failCount = 0;
            CountText.text = $"{hitCount}/{hitCount + failCount}";
        }
        else
        {
            StopCoroutine(barCoroutine);
            NantaScenarioManager.instance.ResetAll();
            NantaScenarioManager.instance.SetScenario(nextScenario);
        }
    }

    public void MissNote()
    {
        failCount++;
        CountText.text = $"{hitCount}/{hitCount + failCount}";
    }

    public override Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 1, MissNote }, { 2, SetCount }, { 3, MissNote }, { 4, StartBar} };
    }
}
