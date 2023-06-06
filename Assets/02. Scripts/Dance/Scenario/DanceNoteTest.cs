using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DanceNoteTest : MonoBehaviour, IScenario
{
    /// <summary>
    /// 공지를 표시하는 텍스트.
    /// </summary>
    [SerializeField] protected TextMeshProUGUI notifyText;
    /// <summary>
    /// 남은 횟수를 표시하는 텍스트.
    /// </summary>
    [SerializeField] protected TextMeshProUGUI countText;

    /// <summary>
    /// 채보 정보가 들어가있는 json 파일.
    /// </summary>
    [SerializeField] protected TextAsset jsonFile;
    /// <summary>
    /// 채보의 음악 오디오 클립.
    /// </summary>
    [SerializeField] protected AudioClip audioClip;
    /// <summary>
    /// 다음 시나리오 번호.
    /// </summary>
    [SerializeField] protected int nextScenario = 4;
    protected IEnumerator barCoroutine;
    /// <summary>
    /// 클리어를 위해 남은 정답 횟수.
    /// </summary>
    protected int clear = 8;

    /// <summary>
    /// 정답 판정이 나오면 남은 횟수 카운트를 감소시킴.
    /// </summary>
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
