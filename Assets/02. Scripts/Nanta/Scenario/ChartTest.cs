using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ChartTest : NoteTest
{
    /// <summary>
    /// 현재 적중한 노트와 판정 실패한 노트의 개수를 알려주는 텍스트.
    /// </summary>
    public TextMeshProUGUI CountText;
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
    public override void Start()
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
    /// <summary>
    /// 노트를 놓쳤을 때 호출되는 함수.
    /// </summary>
    public void MissNote()
    {
        failCount++;
        CountText.text = $"{hitCount}/{hitCount + failCount}";
    }
    public override Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 1, MissNote }, { 2, StartBar } };
    }
}
