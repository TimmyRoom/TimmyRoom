using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���� ���� �ó����� ������ ���� ���� ������Ʈ���� �����ϰų� �����Ѵ�.
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
    /// ���� ���� ������ ����ϴ� Ŭ�����̴�.
    /// </summary>
    public DanceJudgingPoint danceJudgingPoint;

    /// <summary>
    /// ���� ���� Ʈ���� ������ ����ϴ� Ŭ�����̴�.
    /// </summary>
    public DanceAreaManager danceAreaManager;

    /// <summary>
    /// �� ��Ȳ���� �����ϴ� UI�̴�.
    /// </summary>
    public GameObject[] Scenarios;

    /// <summary>
    /// ���� ���� ���� ���� ���ӽð� �� �����̴�.
    /// </summary>
    [Range(0, 1)]
    public float correctVTime = 0.2f;
    [Range(0, 1)]
    public float correctVAmplifier = 0.3f;
    /// <summary>
    /// ���� ���� ���� ���� ���ӽð� �� �����̴�.
    /// </summary>
    [Range(0, 1)]
    public float wrongVTime = 0.4f;
    [Range(0, 1)]
    public float wrongVAmplifier = 0.6f;

    /// <summary>
    /// Instruction���� �߻� ������ �̺�Ʈ Ÿ���� ǥ���ϴ� �������̴�.
    /// </summary>
    public enum EventType
    {
        Hit,
        Fail,
        Start,
        End
    }

    /// <summary>
    /// ���� ���� ������ ���� �̺�Ʈ.
    /// </summary>
    private Dictionary<EventType, UnityEvent> ScenarioEvents =
        new Dictionary<EventType, UnityEvent>(){
        { EventType.Hit, new UnityEvent() },
        { EventType.Fail, new UnityEvent() },
        { EventType.Start, new UnityEvent() },
        { EventType.End, new UnityEvent() }
        };

    /// <summary>
    /// �޺� �޼� �� �߻��ϴ� ȿ���� ����̴�.
    /// </summary>
    public AudioClip[] ComboClips;

    /// <summary>
    /// ��� ������ ����Ǵ� AudioSource�̴�.
    /// </summary>
    public AudioSource MusicAudioSource;

    /// <summary>
    /// �޺� �޼� �� �߻��ϴ� ȿ������ ����Ǵ� AudioSource�̴�.
    /// </summary>
    public AudioSource ComboAudioSource;

    /// <summary>
    /// ���� �������� �ó������� ��ȣ�̴�.
    /// </summary>
    public int currentScenarioNum;

    //int barCombo = 0;

    /// <summary>
    /// ������ ����ϴ� ��ƾ�� �����Ѵ�. �������� ������ ����ȴ�.
    /// </summary>
    IEnumerator SongRoutine;

    //IEnumerator ComboRoutine;

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// �ʱ� ������ ���� �Լ�.
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
    /// SoundManager�� ���� ������ ����Ѵ�.
    /// </summary>
    /// <param name="audioClip">����� ����� Ŭ��.</param>
    /// <param name="barSecond">����� �ҿ� �ð�.</param>
    void StartMusic(AudioClip audioClip, float barSecond)
    {
        SoundManager.instance.PlaySound(audioClip, MusicAudioSource);
    }

    /// <summary>
    /// SoundManager�� ���� �޺� ���带 ����Ѵ�.
    /// </summary>
    IEnumerator PlayComboSound()
    {
        // �޺� ���尡 Ÿ�̹��� ���� �� �ְ� �Ǹ� ����
        //SoundManager.instance.PlaySound(ComboClips[0], ComboAudioSource);
        yield return null;
    }

    //IEnumerator AddComboLoop(float barSecond)
    //{
    //    WaitForSeconds tick = new WaitForSeconds(barSecond);
    //    while (true)
    //    {
    //        yield return tick;
    //        barCombo += 1;
    //        if (barCombo == 2)
    //        {
    //            SoundManager.instance.PlaySound(ComboClips[0], ComboAudioSource);
    //        }
    //        else if (barCombo > 2 && barCombo % 2 == 0)
    //        {
    //            SoundManager.instance.PlaySound(ComboClips[0], ComboAudioSource);
    //        }
    //    }
    //}

    //IEnumerator EndComboLoop(float songLength)
    //{
    //    yield return new WaitForSeconds(songLength);
    //    StopCoroutine(ComboRoutine);
    //}

    //public void ResetCombo()
    //{
    //    barCombo = 0;
    //}

    /// <summary>
    /// �� ��Ʈ�� ���� CommandExecute(time, command) ȣ��.
    /// </summary>
    /// <param name="json">JSON ������.</param>
    /// <param name="audioClip">����� ����.</param>
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

    /// <summary>
    /// ���� �ð� �� ������ ����ϴ� �ڷ�ƾ.
    /// <param name="audioClip">����� ����� Ŭ��.</param>
    /// <param name="waitTime">��� �ð�.</param>
    /// <param name="barSecond">����� �ҿ� �ð�.</param>
    /// </summary>
    IEnumerator PlayChartRoutine(AudioClip audioClip, float waitTime, float barSecond)
    {
        yield return new WaitForSeconds(waitTime);
        StartMusic(audioClip, barSecond);
        StartCoroutine(RecordAndCapture());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
                        StartCoroutine(PlayComboSound());
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
        //barCombo = 0;
        StopCoroutine(SongRoutine);
        danceJudgingPoint.ResetAll();
        SoundManager.instance.StopSound(MusicAudioSource);
    }
}
