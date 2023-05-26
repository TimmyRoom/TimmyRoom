using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 난타 내부 시나리오 전개를 위해 여러 오브젝트들을 생성하거나 제거한다.
/// </summary>
public class NantaScenarioManager : MusicContentTool
{
    public static NantaScenarioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }
    [SerializeField] NantaJudgingLine nantaJudgeLine;
    
    [SerializeField] NantaInstrument nantaInstrument;

    /// <summary>
    /// 각 상황마다 등장하는 UI이다.
    /// </summary>
    public GameObject[] Instructions;

    /// <summary>
    /// Instruction에서 발생 가능한 이벤트 타입.
    /// </summary>
    public enum EventType
    {
        LeftHit,
        LeftFail,
        RightHit,
        RightFail,
        Start,
        End
    }

    /// <summary>
    /// 난타 북 타격 판정에 따른 이벤트.
    /// </summary>
    private Dictionary<EventType, UnityEvent> ScenarioEvents = 
        new Dictionary<EventType, UnityEvent>(){
        { EventType.LeftHit, new UnityEvent() },
        { EventType.LeftFail, new UnityEvent() },
        { EventType.RightHit, new UnityEvent() },
        { EventType.RightFail, new UnityEvent() },
        { EventType.Start, new UnityEvent() },
        { EventType.End, new UnityEvent() }
        };

    /// <summary>
    /// 콤보 달성 시 발생하는 효과음 목록이다.
    /// </summary>
    public AudioClip[] ComboClips;

    /// <summary>
    /// 배경 음원이 재생되는 AudioSource이다.
    /// </summary>
    public AudioSource MusicAudioSource;

    /// <summary>
    /// 콤보 달성 시 발생하는 효과음이 재생되는 AudioSource이다.
    /// </summary>
    public AudioSource ComboAudioSource;

    /// <summary>
    /// 마디의 모든 노트를 성공적으로 처리할 경우 값이 상승한다.
    /// </summary>
    int barCombo = 0;

    /// <summary>
    /// AddComboLoop 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.
    /// </summary>
    IEnumerator ComboRoutine;
    void Start()
    {
        SetScenario(0);
    }
    /// <summary>
    /// 콤보 루틴을 실행하여 일정 시간마다 콤보가 쌓이도록 한다.
    /// </summary>
    /// <param name="audioClip">재생할 오디오 클립.</param>
    /// <param name="barSecond">마디당 소요 시간.</param>
    void StartMusic(AudioClip audioClip, float barSecond)
    {
        SoundManager.instance.SoundPlay(audioClip, MusicAudioSource);
        //ComboRoutine = AddComboLoop(barSecond);
        //StartCoroutine(ComboRoutine);
    }

    /// <summary>
    /// 초마다 콤보를 쌓으며 쌓인 콤보에 따라 효과음 등을 처리한다.
    /// </summary>
    /// <param name="barSecond">마디당 소요 시간.</param>
    /// <returns>서브 루틴.</returns>
    IEnumerator AddComboLoop(float barSecond)
    {
        WaitForSeconds tick = new WaitForSeconds(barSecond);
        while(true)
        {
            yield return tick;
            barCombo += 1;
            if(barCombo == 2)
            {
                SoundManager.instance.SoundPlay(ComboClips[0], ComboAudioSource);
            }
            else if(barCombo > 2 && barCombo % 2 == 0)
            {
                SoundManager.instance.SoundPlay(ComboClips[1], ComboAudioSource);
            }
        }
    }

    /// <summary>
    /// WaitforSecond로 songLength초 만큼 대기 후 콤보 루틴을 종료한다.
    /// </summary>
    /// <param name="songLength">재생한 노래 길이.</param>
    /// <returns>서브 루틴.</returns>
    IEnumerator EndComboLoop(float songLength)
    {
        yield return new WaitForSeconds(songLength);
        StopCoroutine(ComboRoutine);
    }

    /// <summary>
    /// barCombo를 0으로 설정한다.
    /// </summary>
    public void ResetCombo()
    {
        barCombo = 0;
    }

    public override void MoveScene(string sceneName)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveScene(int sceneIndex)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 각 노트에 대해 CommandExecute(time, command) 호출.
    /// </summary>
    /// <param name="json">JSON 데이터.</param>
    /// <param name="audioClip">재생할 음원.</param>
    /// <returns></returns>
    public override GameChart PlayChart(string json, AudioClip audioClip)
    {
        GameChart data = GetScript(json);
        nantaJudgeLine.SetVelocity();
        foreach(var note in data.Notes)
        {
            CommandExecute(data.Offset + Beat2Second(note.Time, data.BPM) + nantaJudgeLine.FallingTime, note.Type);
        }
        
        Debug.Log(Time.time);
        StartCoroutine(PlayChartRoutine(audioClip, nantaJudgeLine.FallingTime, Beat2Second(1, data.BPM)));
        return data;
    }

    IEnumerator PlayChartRoutine(AudioClip audioClip, float waitTime, float barSecond)
    {
        yield return new WaitForSeconds(waitTime);
        StartMusic(audioClip, barSecond);
    }

    /// <summary>
    /// switch구문으로 branch를 나눠 command에 따라 적절한 함수를 실행한다.
    /// </summary>
    /// <param name="time">command가 실행될 기준 시간.</param>
    /// <param name="command">command 구문.</param>
    public override void CommandExecute(float time, string command)
    {
        switch(command)
        {
            case "LeftHand":
            {
                nantaJudgeLine.SpawnNote(time, 0);
                break;
            }
            case "RightHand":
            {
                nantaJudgeLine.SpawnNote(time, 1);
                break;
            }
        }
    }

    /// <summary>
    /// 노트 판정을 내린다.
    /// </summary>
    /// <param name="type">노트의 타입.</param>
    /// <returns>노트 판정 결과.</returns>
    public override int JudgeNote(int type)
    {
        int result = nantaJudgeLine.JudgeNote(type);
        switch(type)
        {
            case 0:
            {
                switch(result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.LeftHit]?.Invoke();
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.LeftFail]?.Invoke();
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                break;
            }
            case 1:
            {
                switch(result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.RightHit]?.Invoke();
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.RightFail]?.Invoke();
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                break;
            }
            default:
            {
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// 현재 시나리오를 scenario 번호에 따라 설정하고 시나리오에 맞는 오브젝트 및 UI를 생성하거나 삭제하고 이벤트 설정을 재설정한다.
    /// </summary>
    /// <param name="scenarioIndex">변경할 시나리오의 Index.</param>
    public override void SetScenario(int scenarioIndex)
    {
        foreach(var instruction in Instructions)
        {
            instruction.SetActive(false);
        }
        Instructions[scenarioIndex].SetActive(true);
        ScenarioEvents[EventType.End].Invoke();
        foreach(var events in ScenarioEvents.Values)
        {
            events.RemoveAllListeners();
        }
        foreach(var KeyPair in Instructions[scenarioIndex].GetComponent<IInstruction>().GetActions())
        {
            ScenarioEvents[(EventType)KeyPair.Key].AddListener(KeyPair.Value);
        }
        ScenarioEvents[EventType.Start].Invoke();
    }
}
