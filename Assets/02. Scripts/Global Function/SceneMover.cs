using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 이동 시 사용되는 공통 함수들을 다루는 클래스.
/// </summary>
public class SceneMover : MonoBehaviour
{
    public static SceneMover instance;
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
    /// 특정 씬으로 이동.
    /// </summary>
    /// <param name="sceneName">이동할 씬 이름.</param>
    void MoveScene(string sceneName)
    {

    }

    /// <summary>
    /// 특정 씬으로 이동.
    /// </summary>
    /// <param name="index">이동할 씬 index.</param>
    void MoveScene(int index)
    {

    }
}
