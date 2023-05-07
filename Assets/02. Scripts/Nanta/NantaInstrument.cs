using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NantaInstrument : MonoBehaviour
{
    /// <summary>
    /// 컨트롤러로 타격 가능한 범위.
    /// </summary>
    public BoxCollider[] Colliders;

    /// <summary>
    /// 악기와 관련된 효과음 목록.
    /// </summary>
    public AudioClip[] InstrumentClips;
        
    /// <summary>
    /// 악기와 관련된 효과음이 나오는 곳이다.
    /// </summary>
    public AudioSource InstrumentAudioSource;

    
    public void OnCollisionEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //변수 int type를 왼쪽 컨트롤러일 경우 0, 오른쪽 컨트롤러일 경우 1로 설정한다.
            //NantaScenarioManager.JudgeNote(type);
            //결과가 Good일 경우 SoundManager.SoundPlay(InstrumentClips[0], InstrumentAudioSource) 호출.
        }
    }
}
