using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractNantaInstrument : MonoBehaviour
{
    /// <summary>
    /// 악기와 관련된 효과음 목록.
    /// </summary>
    public AudioClip[] InstrumentClips;
        
    /// <summary>
    /// 악기와 관련된 효과음이 나오는 곳.
    /// </summary>
    public AudioSource InstrumentAudioSource;
    public NantaJudgingLine Judge;

    /// <summary>
    /// 악기의 초기화를 위한 함수.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// 악기가 사용자에 의해 인터렉션되었을 때 호출되는 함수.
    /// </summary>
    public abstract void GetHitted(int type);
}
