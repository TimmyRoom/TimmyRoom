using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    }
    /// <summary>
    /// 난타 북의 판정을 담당하는 클래스이다.
    /// </summary>
    [SerializeField] private NantaJudgingLine nantaJudgeLine;
    /// <summary>
    /// 난타 악기 오브젝트들을 관리하는 클래스이다.
    /// </summary>
    [SerializeField] private NantaInstrumentManager nantaInstrumentManager;
    /// <summary>
    /// 난타 판정 발생 시 진동이 발생하는 시간.
    /// </summary>
    [Range(0, 1)]
    public float VibrateTime = 0.5f;
    /// <summary>
    /// 난타 판정 발생 시 진동의 세기.
    /// </summary>
    [Range(0, 1)]
    public float VibrateAmplifier = 0.5f;
    /// <summary>
    /// 각 상황마다 등장하는 UI이다.
    /// </summary>
    public GameObject[] Scenarios;

    /// <summary>
    /// Instruction에서 발생 가능한 이벤트 타입을 표현하는 열거형이다.
    /// </summary>
    public enum EventType
    {
        Hit,
        Fail,
        Start,
        SongEnd
    }

    /// <summary>
    /// 난타 북 타격 판정에 따른 이벤트.
    /// </summary>
    private Dictionary<EventType, UnityEvent> ScenarioEvents = 
        new Dictionary<EventType, UnityEvent>(){
        { EventType.Hit, new UnityEvent() },
        { EventType.Fail, new UnityEvent() },
        { EventType.Start, new UnityEvent() },
        { EventType.SongEnd, new UnityEvent() }
        };

    /// <summary>
    /// 배경 음원이 재생되는 AudioSource이다.
    /// </summary>
    public AudioSource MusicAudioSource;

    /// <summary>
    /// 음악을 재생하는 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.
    /// </summary>
    IEnumerator SongRoutine;

    /// <summary>
    /// 음악 종료 후 발생하는 이벤트를 처리하는 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.
    /// </summary>
    IEnumerator StopRoutine;
    void Start()
    {
        Initialize();
    }
    /// <summary>
    /// 초기 설정을 위한 함수.
    /// </summary>
    void Initialize()
    {
        VibrateControl.instance.InitializeController();
        nantaInstrumentManager.Initialize();
        SetScenario(0);
    }

    /// <summary>
    /// 콤보 루틴을 실행하여 일정 시간마다 콤보가 쌓이도록 한다.
    /// </summary>
    /// <param name="audioClip">재생할 오디오 클립.</param>
    /// <param name="barSecond">마디당 소요 시간.</param>
    void StartMusic(AudioClip audioClip, float barSecond)
    {
        SoundManager.instance.PlaySound(audioClip, MusicAudioSource);
    }

    /// <summary>
    /// 음악을 멈추고 콤보 루틴을 종료한다. 
    /// </summary>
    IEnumerator StopMusic(float songLength)
    {
        yield return new WaitForSeconds(songLength + 1f);
        ScenarioEvents[EventType.SongEnd].Invoke();
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
            CommandExecute(data.Offset + Beat2Second(note.Time, data.BPM) + GetWaitTime(), note.Actions);
        }
        SongRoutine = PlayChartRoutine(audioClip, GetWaitTime(), Beat2Second(1, data.BPM));
        StartCoroutine(SongRoutine);
        StopRoutine = StopMusic(data.SongLength);
        StartCoroutine(StopRoutine);
        return data;
    }

    /// <summary>
    /// NantaJudgingLine의 judgingTime을 반환한다.
    /// </summary>
    public override float GetWaitTime()
    {
        return nantaJudgeLine.FallingTime;
    }
    
    /// <summary>
    /// 일정 시간 후 음악을 재생하는 코루틴.
    /// <param name="audioClip">재생할 오디오 클립.</param>
    /// <param name="waitTime">대기 시간.</param>
    /// <param name="barSecond">마디당 소요 시간.</param>
    /// </summary>
    IEnumerator PlayChartRoutine(AudioClip audioClip, float waitTime, float barSecond)
    {
        yield return new WaitForSeconds(waitTime);
        StartMusic(audioClip, barSecond);
        StartCoroutine(RecordAndCapture());
    }

    public override void CommandExecute(float time, List<Action> actions)
    {
        foreach(var action in actions)
        {
            switch(action.Name)
            {
                case "Hit":
                {
                    if(actions.Find(x => x.Name == "Change") != null)
                    {
                        nantaJudgeLine.SpawnNoteWithChange(time, action.Type);
                    }
                    else
                    {
                        nantaJudgeLine.SpawnNote(time, action.Type);
                    }
                    break;
                }
                case "Change":
                {
                    nantaInstrumentManager.ChangeInstrument(time, action.Type);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 악기를 즉시 교체한다.
    /// </summary>
    /// <param name="type">악기의 종류</param>
    public void ChangeInstrumentInstantly(int type)
    {
        nantaInstrumentManager.ChangeInstrument(0f, type);
    }

    public override int JudgeNote(int type, int result)
    {
        switch(type)
        {
            case 0:
            {
                switch(result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(VibrateAmplifier, VibrateTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(VibrateAmplifier, VibrateTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
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
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(VibrateAmplifier, VibrateTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(VibrateAmplifier, VibrateTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
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
                switch(result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(VibrateAmplifier, VibrateTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(VibrateAmplifier, VibrateTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                break;
            }
        }
        return result;
    }

    public override void SetScenario(int scenarioIndex)
    {
        foreach(var scenario in Scenarios)
        {
            scenario.SetActive(false);
        }
        Scenarios[scenarioIndex].SetActive(true);
        if(StopRoutine != null)
        {
            StopCoroutine(StopRoutine);
        }
        foreach(var events in ScenarioEvents.Values)
        {
            events.RemoveAllListeners();
        }

        nantaInstrumentManager.ChangeInstrument(0f, 0);

        foreach(var KeyPair in Scenarios[scenarioIndex].GetComponent<IScenario>().GetActions())
        {
            ScenarioEvents[(EventType)KeyPair.Key].AddListener(KeyPair.Value);
        }
        ScenarioEvents[EventType.Start].Invoke();
    }
    /// <summary>
    /// 씬 시작 상태로 되돌리는 함수.
    /// </summary>
    public override void ResetAll()
    {
        StopCoroutine(SongRoutine);
        nantaJudgeLine.ResetAll();
        nantaInstrumentManager.ResetAll();
        SoundManager.instance.StopSound(MusicAudioSource);
    }
}
