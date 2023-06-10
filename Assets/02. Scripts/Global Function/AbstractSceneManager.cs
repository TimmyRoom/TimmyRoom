using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 씬의 매니저 클래스가 포함할 멤버와 메서드를 담는 추상 클래스.
/// </summary>
public abstract class AbstractSceneManager : MonoBehaviour
{
    /// <summary>
    /// XR Origin의 Main Camera 위치를 담는 변수.
    /// </summary>
    public Transform XRCamera;
    /// <summary>
    /// 씬의 초기 위치를 담는 변수. 기준은 XR Origin의 Main Camera.
    /// </summary>
    public Transform StartTransform;
    /// <summary>
    /// XR Origin의 Camera Offset을 담는 변수.
    /// </summary>
    public Transform XRCameraOffset;
    /// <summary>
    /// 사용자 위치를 초기화하는 함수.
    /// </summary>
    public void ResetPosition()
    {
        XRCameraOffset.position = StartTransform.position - XRCamera.localPosition;
        XRCameraOffset.rotation = Quaternion.Euler(0, StartTransform.rotation.eulerAngles.y - XRCamera.rotation.eulerAngles.y, 0);
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