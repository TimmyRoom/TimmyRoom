using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayConcert : MonoBehaviour, IScenario
{
    [SerializeField] TextMeshProUGUI text;
    [HideInInspector] public TextAsset jsonFile;
    [HideInInspector] public AudioClip audioClip;
    private int count = 0;
    public virtual void OnEnable()
    {
        count = 0;
        text.text = count.ToString();
    }
    public void StartSong()
    {
        NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
    }
    public void SetCount()
    {
        text.text = (++count).ToString();
    }
    public void StopChart()
    {
        NantaScenarioManager.instance.SetScenario(0);
        SoundManager.instance.StopSound(NantaScenarioManager.instance.MusicAudioSource);
    }
    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetCount }, { 2, StartSong }, {3, StopChart } };
    }
}