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

    public enum NoteType
    {
        LeftUpper,
        RightUpper,
        LeftMiddle,
        RightMiddle,
        Front
    }

    public enum NoteResult
    {
        Miss,
        Good
    }

    public UnityEvent[] TriggerEventsLeftUpper;

    public UnityEvent[] TriggerFailEventsLeftUpper;

    public UnityEvent[] TriggerEventsRightUpper;

    public UnityEvent[] TriggerFailEventsRightUpper;

    public UnityEvent[] TriggerEventsLeftMiddle;

    public UnityEvent[] TriggerFailEventsLeftMiddle;

    public UnityEvent[] TriggerEventsRightMiddle;

    public UnityEvent[] TriggerFailEventsRightMiddle;

    public AudioClip[] ComboClips;

    public AudioSource MusicAudioSource;

    public AudioSource ComboAudioSource;

    int barCombo = 0;

    IEnumerator ComboRoutine()
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

    public override void CommandExecute(float time, string command)
    {
        throw new System.NotImplementedException();
    }

    public override float GetWaitTime()
    {
        throw new System.NotImplementedException();
    }

    public override int JudgeNote(int type, int result)
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

    public override void ResetAll()
    {
        throw new System.NotImplementedException();
    }

    public override void SetScenario(int scenarioIndex)
    {
        throw new System.NotImplementedException();
    }

}
