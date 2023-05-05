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
    public void MoveScene(string sceneName)
    {
        //TODO : 메서드 구현.
    }

    /// <summary>
    /// 특정 씬으로 이동.
    /// </summary>
    /// <param name="sceneIndex">이동할 씬 index.</param>
    public void MoveScene(int sceneIndex)
    {
        //TODO : 메서드 구현.
    }
}
