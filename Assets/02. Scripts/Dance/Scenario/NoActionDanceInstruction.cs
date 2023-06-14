using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoActionDanceInstruction : MonoBehaviour, IScenario
{
    [SerializeField] DanceChartTest danceChart;
    /// <summary>
    /// 해당 인스트럭션에서 발생하는 액션들의 리스트를 반환한다.
    /// </summary>
    /// <returns>인스트럭션에서 발생하는 액션들의 리스트</returns>
    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>();
    }

    /// <summary>
    /// 메인 컨텐츠의 채보를 설정한다.
    /// </summary>
    /// <param name="jsonFile">채보.</param>
    public void SetJSON(TextAsset jsonFile)
    {
        danceChart.jsonFile = jsonFile;
    }
    /// <summary>
    /// 메인 컨텐츠의 음악을 설정한다.
    /// </summary>
    /// <param name="audioClip">음악.</param>
    public void SetSong(AudioClip audioClip)
    {
        danceChart.audioClip = audioClip;
    }
}
