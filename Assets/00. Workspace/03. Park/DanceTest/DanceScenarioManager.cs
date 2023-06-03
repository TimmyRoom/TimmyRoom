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

    int barCombo = 0;

    IEnumerator SongRoutine;

    IEnumerator ComboRoutine;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        throw new System.NotImplementedException();
    }

    void StartMusic(AudioClip audioClip, float barSecond)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator AddComboLoop(float barSecond)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator EndComboLoop(float songLength)
    {
        throw new System.NotImplementedException();
    }

    public void ResetCombo()
    {
        throw new System.NotImplementedException();
    }

    public override GameChart PlayChart(string json, AudioClip audioClip)
    {
        throw new System.NotImplementedException();
    }

    public override float GetWaitTime()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator PlayChartRoutine(AudioClip audioClip, float waitTime, float barSecond)
    {
        throw new System.NotImplementedException();
    }

    public override void CommandExecute(float time, List<Action> actions)
    {
        throw new System.NotImplementedException();
    }

    public override int JudgeNote(int type, int result)
    {
        throw new System.NotImplementedException();
    }

    public override void SetScenario(int scenarioIndex)
    {
        throw new System.NotImplementedException();
    }

    public override void ResetAll()
    {
        throw new System.NotImplementedException();
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
