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
        DontDestroyOnLoad(this);
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
        End
    }

    /// <summary>
    /// 난타 북 타격 판정에 따른 이벤트.
    /// </summary>
    private Dictionary<EventType, UnityEvent> ScenarioEvents = 
        new Dictionary<EventType, UnityEvent>(){
        { EventType.Hit, new UnityEvent() },
        { EventType.Fail, new UnityEvent() },
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
    /// 
    int barCombo = 0;
    /// <summary>
    /// 음악을 재생하는 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.
    /// </summary>
    IEnumerator SongRoutine;

    /// <summary>
    /// AddComboLoop 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.
    /// </summary>
    IEnumerator ComboRoutine;
    void Start()
    {
        Initialize();
    }
    /// <summary>
    /// 초기 설정을 위한 함수.
    /// </summary>
    void Initialize()
    {
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
                SoundManager.instance.PlaySound(ComboClips[0], ComboAudioSource);
            }
            else if(barCombo > 2 && barCombo % 2 == 0)
            {
                SoundManager.instance.PlaySound(ComboClips[1], ComboAudioSource);
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
        return data;
    }

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
    }

    public override void CommandExecute(float time, List<Action> actions)
    {
        foreach(var action in actions)
        {
            switch(action.Name)
            {
                case "Hit":
                {
                    nantaJudgeLine.SpawnNote(time, action.Type);
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

        ScenarioEvents[EventType.End].Invoke();
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
        barCombo = 0;
        //StopCoroutine(ComboRoutine);
        StopCoroutine(SongRoutine);
        nantaJudgeLine.ResetAll();
        SoundManager.instance.StopSound(MusicAudioSource);
    }

    public override void MoveScene(string sceneName)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveScene(int sceneIndex)
    {
        throw new System.NotImplementedException();
    }
}
