using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 음악과 관련하여 BPM을 분석해 박자 단위의 시간 체계를 초 단위 시간으로 변환하며, 박자 단위로 명령을 입력한 csv 파일을 읽어 Dictionary 형태의 자료구조로 변환한다.
/// 각 씬의 매너지 클래스 중 일부가 상속하며 AbstractSceneManager를 상속받는다.
/// </summary>
public abstract class MusicContentTool : AbstractSceneManager
{
    /// <summary>
    /// 마디 단위를 받아 BPM에 따라 정확한 초를 계산한다.
    /// </summary>
    /// <param name="beat">마디 수.</param>
    /// <param name="BPM">BPM</param>
    /// <returns></returns>
    float Beat2Second(float beat, float BPM) 
    {
        return beat * 60 / BPM;
    }

    /// <summary>
    /// 초 단위를 받아 BPM에 따라 정확한 마디를 계산한다.
    /// </summary>
    /// <param name="second">초 단위.</param>
    /// <param name="BPM">BPM</param>
    /// <returns></returns>
    float Second2Beat(float second, float BPM) 
    {
		return second * BPM /60;
    }

    /// <summary>
    /// 각 판정 이벤트들의 정보가 포함된 JSON파일을 받아 Dictionary 형태로 변환한다.
    /// </summary>
    /// <param name="jsonData">JSON 데이터.</param>
    /// <returns></returns>
    Dictionary<float, string> GetScript(string jsonData) 
    {
		return JsonUtility.FromJson<Dictionary<float, string>>(jsonData);
    }

    /// <summary>
    /// 각 노트에 대해 CommandExecute(time, command) 호출.
    /// </summary>
    /// <param name="json"></param>
    public virtual void PlayChart(string json)
    {
        Dictionary<float, string> data = GetScript(json);
    }

    /// <summary>
    /// switch구문으로 branch를 나눠 command에 따라 적절한 함수를 실행한다.
    /// </summary>
    /// <param name="time">command가 실행될 기준 시간.</param>
    /// <param name="command">command 구문.</param>
    public virtual void CommandExecute(float time, string command)
    {
        //
    }

    /// <summary>
    /// 노트 판정을 내린다.
    /// </summary>
    /// <param name="type">노트의 타입.</param>
    /// <returns>노트 판정 결과.</returns>
    public virtual int JudgeNote(int type)
    {
        return 0;
    }

    public abstract override void SetScenario(int scenarioIndex);
}
