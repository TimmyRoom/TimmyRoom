using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 씬의 매니저 클래스가 포함할 멤버와 메서드를 담는 추상 클래스.
/// </summary>
public abstract class AbstractSceneManager : MonoBehaviour
{
    /// <summary>
    /// XR 헤드마운트 카메라 위치를 담는 변수.
    /// </summary>
    public Transform XRHead;
    /// <summary>
    /// 씬의 초기 위치를 담는 변수.
    /// </summary>
    public Transform SceneObject;
    /// <summary>
    /// XR Rig를 담는 변수.
    /// </summary>
    public Transform XRRig;
    /// <summary>
    /// 사용자 위치를 초기화하는 함수.
    /// </summary>
    public void ResetPosition()
    {
        Debug.Log("Init");
        XRRig.position = SceneObject.position - XRHead.localPosition;
        XRRig.rotation = Quaternion.Euler(0, SceneObject.rotation.eulerAngles.y - XRHead.rotation.eulerAngles.y, 0);
    }
    /// <summary>
    /// 특정 씬으로 이동하는 로직을 담은 함수.
    /// </summary>
    /// <param name="sceneName">이동할 씬의 이름.</param>
    public abstract void MoveScene(string sceneName);

    /// <summary>
    /// 특정 씬으로 이동하는 로직을 담은 함수.
    /// </summary>
    /// <param name="sceneIndex">이동할 씬의 index.</param>
    public abstract void MoveScene(int sceneIndex);
}