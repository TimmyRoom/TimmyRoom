using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 씬의 매니저 클래스가 포함할 멤버와 메서드를 담는 추상 클래스.
/// </summary>
public abstract class AbstractSceneManager : MonoBehaviour
{
    /// <summary>
    /// 초기화시 필요한 로직들을 담는 함수.
    /// </summary>
    void Init()
    {
        //TODO : Init 완성.
    }

    /// <summary>
    /// 특정 씬으로 이동하는 로직을 담은 함수.
    /// </summary>
    /// <param name="sceneName">이동할 씬의 이름.</param>
    public void MoveScene(string sceneName)
    {
        //TODO : using 추가 소요를 줄이고, SceneMover 싱글톤 사용 유도
    }

    /// <summary>
    /// 특정 씬으로 이동하는 로직을 담은 함수.
    /// </summary>
    /// <param name="sceneIndex">이동할 씬의 index.</param>
    public void MoveScene(int sceneIndex)
    {
        //TODO : using 추가 소요를 줄이고, SceneMover 싱글톤 사용 유도
    }
}