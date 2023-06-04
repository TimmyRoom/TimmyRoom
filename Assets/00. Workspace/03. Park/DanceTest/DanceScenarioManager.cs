using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        DontDestroyOnLoad(this);
    }

    [SerializeField] DanceJudgingPoint danceJudgingPoint;
    public DanceAreaManager danceAreaManager;

    public GameObject[] Scenarios;

    public enum EventType
    {
        Hit,
        Fail,
        Start,
        End
    }

    Dictionary<EventType, UnityEvent> ScenarioEvents =
        new Dictionary<EventType, UnityEvent>(){
        { EventType.Hit, new UnityEvent() },
        { EventType.Fail, new UnityEvent() },
        { EventType.Start, new UnityEvent() },
        { EventType.End, new UnityEvent() }
        };

    public AudioClip[] ComboClips;

    public AudioSource MusicAudioSource;

    public AudioSource ComboAudioSource;

    public int currentScenarioNum;

    int barCombo = 0;

    IEnumerator SongRoutine;

    IEnumerator ComboRoutine;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        danceAreaManager.Initialize();
        SetScenario(0);
    }

    void StartMusic(AudioClip audioClip, float barSecond)
    {
        SoundManager.instance.PlaySound(audioClip, MusicAudioSource);
    }

    IEnumerator AddComboLoop(float barSecond)
    {
        WaitForSeconds tick = new WaitForSeconds(barSecond);
        while (true)
        {
            yield return tick;
            barCombo += 1;
            if (barCombo == 2)
            {
                SoundManager.instance.PlaySound(ComboClips[0], ComboAudioSource);
            }
            else if (barCombo > 2 && barCombo % 2 == 0)
            {
                SoundManager.instance.PlaySound(ComboClips[1], ComboAudioSource);
            }
        }
    }

    IEnumerator EndComboLoop(float songLength)
    {
        yield return new WaitForSeconds(songLength);
        StopCoroutine(ComboRoutine);
    }

    public void ResetCombo()
    {
        barCombo = 0;
    }

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

    IEnumerator PlayChartRoutine(AudioClip audioClip, float waitTime, float barSecond)
    {
        yield return new WaitForSeconds(waitTime);
        StartMusic(audioClip, barSecond);
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
            case 12:
            {
                switch (result)
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
            case 13:
            {
                switch (result)
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
            case 21:
            {
                switch (result)
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
            case 22:
            {
                switch (result)
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
            case 23:
            {
                switch (result)
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
        barCombo = 0;
        StopCoroutine(SongRoutine);
        danceJudgingPoint.ResetAll();
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
