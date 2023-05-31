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
    /// <param name="BPM">BPM.</param>
    /// <returns>주어진 마디를 초 단위 시간으로 환산한 값.</returns>
    public float Beat2Second(float beat, float BPM) 
    {
        return beat * 60 / BPM;
    }

    /// <summary>
    /// 초 단위를 받아 BPM에 따라 정확한 마디를 계산한다.
    /// </summary>
    /// <param name="second">초 단위 시간.</param>
    /// <param name="BPM">BPM.</param>
    /// <returns>주어진 초 단위 시간을 마디 수로 환산한 값.</returns>
    public float Second2Beat(float second, float BPM) 
    {
		return second * BPM /60;
    }

    /// <summary>
    /// 각 판정 이벤트들의 정보가 포함된 JSON파일을 받아 Dictionary 형태로 변환한다.
    /// </summary>
    /// <param name="jsonData">JSON 데이터.</param>
    /// <returns>Dictionary 자료구조.</returns>
    public GameChart GetScript(string jsonData) 
    {
		return JsonUtility.FromJson<GameChart>(jsonData);
    }

    /// <summary>
    /// 각 노트에 대해 CommandExecute(time, command) 호출.
    /// </summary>
    /// <param name="json">JSON 데이터.</param>
    public virtual GameChart PlayChart(string json, AudioClip audio)
    {
        GameChart data = GetScript(json);
        foreach(var note in data.Notes)
        {
            CommandExecute(Beat2Second(note.Time, data.BPM), note.Type);
        }
        return data;
    }

    /// <summary>
    /// 음악 재생 전 대기하는 시간을 기록한다.
    /// 이는 노트 낙하 및 등장 시간으로 인해 WaitforSecond의 파라미터로 음수 시간이 할당되는 것을 방지하기 위함이다.
    /// </summary>
    /// <returns>노트 이펙트 생성에 필요한 최대 시간</returns>
    public abstract float GetWaitTime();

    /// <summary>
    /// 노트 판정에 따른 이벤트를 발생시킨다.
    /// </summary>
    /// <param name="type">노트의 타입.</param>
    /// <param name="result">노트 판정 결과.</param>
    /// <returns>노트 판정 결과.</returns>
    public abstract int JudgeNote(int type, int result);

    /// <summary>
    /// switch구문으로 branch를 나눠 command에 따라 적절한 함수를 실행한다.
    /// </summary>
    /// <param name="time">command가 실행될 기준 시간.</param>
    /// <param name="command">command 구문.</param>
    public abstract void CommandExecute(float time, string command);
    /// <summary>
    /// 씬의 상태를 채보 시작 이전 상태로 되돌린다.
    /// </summary>
    public abstract void ResetAll();

    /// <summary>
    /// 현재 시나리오를 scenario 번호에 따라 설정하고 시나리오에 맞는 오브젝트 및 데이터, UI를 생성하거나 삭제한다.
    /// </summary>
    /// <param name="scenarioIndex">변경할 시나리오의 Index.</param>
    public abstract void SetScenario(int scenarioIndex);
}
