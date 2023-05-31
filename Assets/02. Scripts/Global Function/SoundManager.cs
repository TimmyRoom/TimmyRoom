using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 원하는 오디오 클립을 재생하는 클래스. 
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
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
    /// 설정한 효과음 clip을 source 위치에서 재생한다.
    /// </summary>
    /// <param name="clip">재생할 AudioClip.</param>
    /// <param name="source">클립이 재생될 AudioSource.</param>
    public void PlaySound(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
    }
    /// <summary>
    /// 설정한 AudioSource의 재생 음악을 멈춘다.
    /// <param name="source">멈추고자 하는 AudioSource.</param>
    /// </summary>
    public void StopSound(AudioSource source)
    {
        source.Stop();
    }
}
