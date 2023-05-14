using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 사용자가 컨트롤러 Ray를 통해 상호작용하여 캔버스 내부의 오브젝트들의 Unity Event를 Invoke하는 클래스.
/// 사용 시에는 각 UI 오브젝트에 스크립트 컴포넌트를 추가하고 이벤트를 지정해준다.
/// </summary>
public class UISelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public float InteractionTime = 2.0f;
	public UnityEvent[] UIEvents;

	private bool mbButtonPressed = false;
	private float mPressTime = 0.0f;

    /// <summary>
    /// 사용자의 컨트롤러 Ray가 UI 오브젝트(버튼 등)로 들어왔을 때 시간을 잰다.
	/// 버튼이 한 번이라도 눌리지 않았을 경우에만 시간을 측정한다.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
	{
		if (!mbButtonPressed)
		{
            mPressTime = Time.time;
		}
	}

    /// <summary>
    /// 사용자의 컨트롤러 Ray가 UI 오브젝트를 나갔을 때 기존 루틴을 중단한다.
    /// 눌린 시간을 초기화하고 기존에 버튼이 눌린 적이 있으면 그 기록을 초기화한다.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
	{
        mPressTime = 0.0f;
        mbButtonPressed = false;
    }

    /// <summary>
    /// Ray가 오브젝트에 들어온 시간을 측정하여 일정 시간동안 입력이 들어왔는지 체크한다.
    /// class 내 설정된 시간이 지나면 변수로 할당된 events들을 Invoke한다.
	/// 중복 입력을 막기 위해 버튼이 입력되었음을 기록해준다.
    /// </summary>
    private void Update()
	{
		if(!mbButtonPressed && mPressTime > 0.0f)
		{
			if(Time.time - mPressTime >= InteractionTime)
			{
				foreach(var UIEvent in UIEvents)
				{
					UIEvent?.Invoke();
				}
                mbButtonPressed = true;
			}
		}
	}
}
