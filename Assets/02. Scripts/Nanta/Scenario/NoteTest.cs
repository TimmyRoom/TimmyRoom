using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NoteTest : MonoBehaviour, IScenario
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextAsset jsonFile;
    [SerializeField] AudioClip audioClip;
    [SerializeField] int count = 7;
    private IEnumerator barCoroutine;
    public void Start()
    {
        text.text = count.ToString();
    }
    public void SetCount()
    {
        text.text = (--count).ToString();
        if(count <= 0)
        {
            StopCoroutine(barCoroutine);
            NantaScenarioManager.instance.ResetAll();
            NantaScenarioManager.instance.SetScenario(3);
        }
    }
    public void StartBar()
    {
        barCoroutine = BarCoroutine(1f);
        StartCoroutine(barCoroutine);
    }
    IEnumerator BarCoroutine(float time)
    {
        GameChart chart = NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
        while (count > 0)
        {
            yield return new WaitForSeconds(240 / chart.BPM);
            NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
        }
    }
    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 2, SetCount }, { 4, StartBar} };
    }
}
