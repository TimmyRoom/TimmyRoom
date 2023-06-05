using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DanceNoteTest : MonoBehaviour, IScenario
{
    [SerializeField] protected TextMeshProUGUI notifyText;
    [SerializeField] protected TextMeshProUGUI countText;
    [SerializeField] protected TextAsset jsonFile;
    [SerializeField] protected AudioClip audioClip;
    [SerializeField] protected int nextScenario = 4;
    protected IEnumerator barCoroutine;
    protected int clear = 8;

    public virtual void SetCount()
    {
        countText.text = (--clear).ToString();
        if (clear < 0)
        {
            clear = 0;
            countText.text = clear.ToString();
        }
    }
    /// <summary>
    /// 채보와 음악을 재생하는 함수.
    /// </summary>
    public void StartBar()
    {
        countText.text = clear.ToString();
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
            GameChart chart = DanceScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
            yield return new WaitForSeconds(chart.SongLength);
            StartCoroutine(MusicCoroutine());
        } while (clear > 0);
    }
    /// <summary>
    /// 채보 재생을 위해 실행되는 코루틴.
    /// </summary>
    protected virtual IEnumerator MusicCoroutine()
    {
        yield return new WaitForSeconds(DanceScenarioManager.instance.GetWaitTime());
        if (clear <= 0)
        {
            StopCoroutine(barCoroutine);
            DanceScenarioManager.instance.ResetAll();
            DanceScenarioManager.instance.SetScenario(nextScenario);
        }
    }
    /// <summary>
    /// 해당 인스트럭션에서 발생하는 액션들의 리스트를 반환한다.
    /// </summary>
    /// <returns>인스트럭션에서 발생하는 액션들의 리스트</returns>
    public virtual Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 2, StartBar } };
    }
}
