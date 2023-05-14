using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NantaInstrument : MonoBehaviour
{
    /// <summary>
    /// 악기와 관련된 효과음 목록.
    /// </summary>
    public AudioClip[] InstrumentClips;
        
    /// <summary>
    /// 악기와 관련된 효과음이 나오는 곳이다.
    /// </summary>
    public AudioSource InstrumentAudioSource;

    [SerializeField] NantaScenarioManager manager;

    
    public void OnTriggerEnter(Collider other)
    {
        int result = 0;
        if(other.CompareTag("LeftController"))
        {
            Debug.Log("Hit Left");
            result = manager.JudgeNote(0);
        }
        else if(other.CompareTag("RightController"))
        {
            Debug.Log("Hit Right");
            result = manager.JudgeNote(1);
        }
        if(result == 0)
        {
            //SoundManager.instance.SoundPlay(InstrumentClips[0], InstrumentAudioSource);
        }
    }
}
