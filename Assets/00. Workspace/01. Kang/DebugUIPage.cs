using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DebugUIPage : MonoBehaviour, IScenario
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextAsset jsonFile;
    [SerializeField] AudioClip audioClip;
    private int count = 0;
    public void StartSong()
    {
        NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
    }
    public void UP()
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
        return new Dictionary<int, UnityAction>() { { 0, UP }, { 2, StartSong}, {3, StopChart} };
    }
}
