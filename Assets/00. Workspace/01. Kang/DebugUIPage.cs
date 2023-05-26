using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DebugUIPage : MonoBehaviour, IInstruction
{
    int count = 0;
    public TextMeshProUGUI text;
    [SerializeField] TextAsset jsonFile;
    [SerializeField] AudioClip audioClip;
    public void StartSong()
    {
        NantaScenarioManager.instance.PlayChart(jsonFile.text, audioClip);
    }
    public void UP()
    {
        text.text = (++count).ToString();
    }

    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, UP }, { 2, UP }, {4, StartSong} };
    }
}
