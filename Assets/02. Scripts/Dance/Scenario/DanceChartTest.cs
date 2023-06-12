using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DanceChartTest : DanceNoteTest
{
    /// <summary>
    /// 다음 시나리오로 넘어가기 위해 필요한 적중률.
    /// </summary>
    [Range(0, 1)]
    [SerializeField] float clearRate = 0.6f;
    /// <summary>
    /// 실패 시 등장하는 텍스트 문구들.
    /// </summary>
    [SerializeField] List<string> failTexts;
    /// <summary>
    /// 적중 횟수.
    /// </summary>
    private int hitCount = 0;
    /// <summary>
    /// 실패 횟수.
    /// </summary>
    private int failCount = 0;

    public void Start()
    {
        countText.text = $"{hitCount}/{hitCount + failCount}";
    }

    /// <summary>
    /// 정답 판정이 나오면 정답 횟수 카운트를 증가시킴.
    /// </summary>
    public override void SetCount()
    {
        hitCount++;
        countText.text = $"{hitCount}/{hitCount + failCount}";
    }

    protected override IEnumerator MusicCoroutine()
    {
        yield return new WaitForSeconds(DanceScenarioManager.instance.GetWaitTime());
        float hitRate = (float)hitCount / (hitCount + failCount);
        if (hitRate < clearRate && clear > 0)
        {
            notifyText.text = failTexts[failTexts.Count - (clear--)];
            hitCount = 0;
            failCount = 0;
            countText.text = $"{hitCount}/{hitCount + failCount}";
        }
        else
        {
            StopCoroutine(barCoroutine);
            DanceScenarioManager.instance.ResetAll();
            DanceScenarioManager.instance.SetScenario(nextScenario);
        }
    }

    /// <summary>
    /// 노트를 놓쳤을 때 호출되는 함수.
    /// </summary>
    public void MissNote()
    {
        failCount++;
        countText.text = $"{hitCount}/{hitCount + failCount}";
    }

    public override Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 1, MissNote }, { 2, StartBar } };
    }
}
