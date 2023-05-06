using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// XR Origin의 ActionBasedController와 연결하여 해당 컨트롤러에 원하는 시간과 강도만큼의 진동을 줄 수 있는 클래스. 
/// </summary>
public class VibrateControl : MonoBehaviour
{
    public static VibrateControl instance;

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

    [SerializeField] private ActionBasedController rightController;
    [SerializeField] private ActionBasedController leftController;

    /// <summary>
    /// 오른쪽 컨트롤러가 활성화된 상태인 경우 오른쪽 컨트롤러에 진동을 재생합니다.
    /// </summary>
    /// <param name="amplitude">0.0에서 1.0 사이의 진동 강도를 지정합니다.</param>
    /// <param name="duration">몇 초동안 진동을 재생할 것인지를 지정합니다.</param>
    public IEnumerator CustomVibrateRight(float amplitude, float duration)
    {
        if (rightController != null)
        {
            rightController.SendHapticImpulse(amplitude, duration);
        }
        else
        {
            Debug.LogError("right controller isn't avaliable.");
        }
        yield return null;
    }

    /// <summary>
    /// 왼쪽 컨트롤러가 활성화된 상태인 경우 왼쪽 컨트롤러에 진동을 재생합니다.
    /// </summary>
    /// <param name="amplitude">0.0에서 1.0 사이의 진동 강도를 지정합니다.</param>
    /// <param name="duration">몇 초동안 진동을 재생할 것인지를 지정합니다.</param>
    public IEnumerator CustomVibrateLeft(float amplitude, float duration)
    {
        if (leftController != null)
        {
            leftController.SendHapticImpulse(amplitude, duration);
        }
        else
        {
            Debug.LogError("left controller isn't avaliable.");
        }

        yield return null;
    }
}
