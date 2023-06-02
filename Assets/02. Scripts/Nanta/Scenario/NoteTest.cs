using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NoteTest : HitTest, IScenario
{
    [SerializeField] protected TextAsset jsonFile;
    [SerializeField] protected AudioClip audioClip;
    [SerializeField] protected int nextScenario = 3;
    protected IEnumerator barCoroutine;
    public override void SetCount()
    {
        text.text = (--count).ToString();
    }
    /// <summary>
    /// 채보와 음악을 재생하는 함수.
    /// </summary>
    public void StartBar()
    {
        barCoroutine = BarCoroutine();
        StartCoroutine(barCoroutine);
    }
    /// <summary>
    /// 음악 재생을 위해 실행되는 코루틴.
    /// </summary>
    protected IEnumerator BarCoroutine()
    {
        do
        {
            GameChart chart = NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
            yield return new WaitForSeconds(chart.SongLength);
            StartCoroutine(MusicCoroutine());
        } while (count > 0);
    }
    /// <summary>
    /// 채보 재생을 위해 실행되는 코루틴.
    /// </summary>
    protected virtual IEnumerator MusicCoroutine()
    {
        yield return new WaitForSeconds(NantaScenarioManager.instance.GetWaitTime());
        if(count <= 0)
        {
            StopCoroutine(barCoroutine);
            NantaScenarioManager.instance.ResetAll();
            NantaScenarioManager.instance.SetScenario(nextScenario);
        }
    }
    /// <summary>
    /// 해당 인스트럭션에서 발생하는 액션들의 리스트를 반환한다.
    /// </summary>
    /// <returns>인스트럭션에서 발생하는 액션들의 리스트</returns>
    public override Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 2, StartBar } };
    }
}
