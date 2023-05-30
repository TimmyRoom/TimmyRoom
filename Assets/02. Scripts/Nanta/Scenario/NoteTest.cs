using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NoteTest : MonoBehaviour, IScenario
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected TextAsset jsonFile;
    [SerializeField] protected AudioClip audioClip;
    [SerializeField] protected int count = 7;
    [SerializeField] protected int nextScenario = 3;
    protected IEnumerator barCoroutine;
    protected virtual void Start()
    {
        text.text = count.ToString();
    }
    public virtual void SetCount()
    {
        text.text = (--count).ToString();
    }
    public void StartBar()
    {
        barCoroutine = BarCoroutine();
        StartCoroutine(barCoroutine);
    }
    protected IEnumerator BarCoroutine()
    {
        do
        {
            GameChart chart = NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
            yield return new WaitForSeconds(chart.SongLength);
            StartCoroutine(MusicCoroutine());
        } while (count > 0);
    }
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
    public virtual Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 2, SetCount }, { 4, StartBar} };
    }
}
