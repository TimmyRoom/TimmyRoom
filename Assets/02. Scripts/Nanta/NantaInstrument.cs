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

    
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LeftController"))
        {
            Debug.Log("Hit Left");
            //int result = NantaScenarioManager.instance.JudgeNote(0);
            //결과가 Good일 경우 SoundManager.SoundPlay(InstrumentClips[0], InstrumentAudioSource) 호출.
        }
        else if(other.CompareTag("RightController"))
        {
            Debug.Log("Hit Right");
            //int result = NantaScenarioManager.instance.JudgeNote(1);
            //결과가 Good일 경우 SoundManager.SoundPlay(InstrumentClips[0], InstrumentAudioSource) 호출.

        }
    }
}
