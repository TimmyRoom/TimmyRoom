using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 댄스 내부 시나리오 전개를 위해 여러 오브젝트들을 생성하거나 제거한다.
/// </summary>
public class DanceScenarioManager : MusicContentTool
{
    public static DanceScenarioManager instance;
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
        //DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 댄스 포즈 판정을 담당하는 클래스이다.
    /// </summary>
    public DanceJudgingPoint danceJudgingPoint;

    /// <summary>
    /// 댄스 포즈 트리거 구역을 담당하는 클래스이다.
    /// </summary>
    public DanceAreaManager danceAreaManager;

    /// <summary>
    /// 각 상황마다 등장하는 UI이다.
    /// </summary>
    public GameObject[] Scenarios;

    /// <summary>
    /// 정답 판정 시의 진동 지속시간 및 세기이다.
    /// </summary>
    [Range(0, 1)]
    public float correctVTime = 0.2f;
    [Range(0, 1)]
    public float correctVAmplifier = 0.3f;
    /// <summary>
    /// 오답 판정 시의 진동 지속시간 및 세기이다.
    /// </summary>
    [Range(0, 1)]
    public float wrongVTime = 0.4f;
    [Range(0, 1)]
    public float wrongVAmplifier = 0.6f;

    /// <summary>
    /// Instruction에서 발생 가능한 이벤트 타입을 표현하는 열거형이다.
    /// </summary>
    public enum EventType
    {
        Hit,
        Fail,
        Start,
        End
    }

    /// <summary>
    /// 배경 음원이 재생되는 AudioSource이다.
    /// </summary>
    public AudioSource MusicAudioSource;

    /// <summary>
    /// 현재 진행중인 시나리오의 번호이다.
    /// </summary>
    public int currentScenarioNum;

    /// <summary>
    /// 음악을 재생하는 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.
    /// </summary>
    IEnumerator SongRoutine;

    /// <summary>
    /// 댄스 포즈 판정에 따른 이벤트.
    /// EventType에 따라 적절한 이벤트를 발생시킨다.
    /// </summary>
    private Dictionary<EventType, UnityEvent> ScenarioEvents =
        new Dictionary<EventType, UnityEvent>(){
        { EventType.Hit, new UnityEvent() },
        { EventType.Fail, new UnityEvent() },
        { EventType.Start, new UnityEvent() },
        { EventType.End, new UnityEvent() }
        };

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 초기 설정을 위한 함수.
    /// </summary>
    void Initialize()
    {
        danceAreaManager.Initialize();
        danceAreaManager.area.DisableGuide();
        danceJudgingPoint.JudgePointGuide.SetActive(false);
        SetScenario(0);
        ResetPosition();
    }

    /// <summary>
    /// 각 노트에 대해 CommandExecute(time, command) 호출.
    /// </summary>
    /// <param name="json">차트 JSON 데이터.</param>
    /// <param name="audioClip">재생할 음원.</param>
    public override GameChart PlayChart(string json, AudioClip audioClip)
    {
        GameChart data = GetScript(json);
        danceJudgingPoint.SetVelocity();
        foreach (var note in data.Notes)
        {
            CommandExecute(data.Offset + Beat2Second(note.Time, data.BPM) + GetWaitTime(), note.Actions);
        }
        SongRoutine = PlayChartRoutine(audioClip, GetWaitTime(), Beat2Second(1, data.BPM));
        StartCoroutine(SongRoutine);
        return data;
    }

    public override float GetWaitTime()
    {
        return danceJudgingPoint.FallingTime;
    }

    public override void CommandExecute(float time, List<Action> actions)
    {
        foreach (var action in actions)
        {
            switch (action.Name)
            {
                case "Hit":
                    {
                        danceJudgingPoint.SpawnNote(time, action.Type);
                        break;
                    }
            }
        }
    }

    public override int JudgeNote(int type, int result)
    {
        switch (type)
        {
            case 11:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 12:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                     {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 13:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 21:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 22:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 23:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 31:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 32:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            case 33:
            {
                switch (result)
                {
                    case 1:
                    {
                        ScenarioEvents[EventType.Hit]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(correctVAmplifier, correctVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(correctVAmplifier, correctVTime));
                        break;
                    }
                    case 0:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    case -1:
                    {
                        ScenarioEvents[EventType.Fail]?.Invoke();
                        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(wrongVAmplifier, wrongVTime));
                        StartCoroutine(VibrateControl.instance.CustomVibrateRight(wrongVAmplifier, wrongVTime));
                        break;
                    }
                    default:
                    {
                        Debug.Assert(false);
                        break;
                    }
                }
                break;
            }
            default:
            {
                Debug.Assert(false);
                break;
            }
        }
        return result;
    } 

    public override void SetScenario(int scenarioIndex)
    {
        ResetPosition();
        foreach (var scenario in Scenarios)
        {
            scenario.SetActive(false);
        }
        Scenarios[scenarioIndex].SetActive(true);

        currentScenarioNum = scenarioIndex;

        ScenarioEvents[EventType.End].Invoke();
        foreach (var events in ScenarioEvents.Values)
        {
            events.RemoveAllListeners();
        }
        foreach (var KeyPair in Scenarios[scenarioIndex].GetComponent<IScenario>().GetActions())
        {
            ScenarioEvents[(EventType)KeyPair.Key].AddListener(KeyPair.Value);
        }
        ScenarioEvents[EventType.Start].Invoke();
    }

    public override void ResetAll()
    {
        StopCoroutine(SongRoutine);
        danceJudgingPoint.ResetAll();
        SoundManager.instance.StopSound(MusicAudioSource);
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
    }

    /// <summary>
    /// SoundManager를 통해 음악을 재생한다.
    /// </summary>
    /// <param name="audioClip">재생할 오디오 클립.</param>
    /// <param name="barSecond">마디당 소요 시간.</param>
    void StartMusic(AudioClip audioClip, float barSecond)
    {
        SoundManager.instance.PlaySound(audioClip, MusicAudioSource);
    }
}