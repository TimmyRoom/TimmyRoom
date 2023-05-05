using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 내부를 촬영하고 촬영된 영상을 저장하는 클래스.
/// </summary>
public class SceneRecorder : MonoBehaviour
{
    public static SceneRecorder instance;
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

    string filePath;

    /// <summary>
    /// filePath를 변경한다.
    /// </summary>
    /// <param name="path">변경할 filePath.</param>
    public void SetPath(string path)
    {
        //
    }

    /// <summary>
    /// 사용자가 바라보는 화면을 캡처하는 함수.
    /// </summary>
    /// <returns>저장 성공여부.</returns>
    public bool Capture()
    {
        return false;
    }

    /// <summary>
    /// 사용자를 바라보는 바깥의 가상 카메라를 캡처하는 함수.
    /// </summary>
    /// <returns>저장 성공여부.</returns>
    public bool CaptureSelf()
    {
        return false;
    }
}
