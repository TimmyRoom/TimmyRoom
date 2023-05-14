using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 사용자가 컨트롤러 Ray를 통해 상호작용하여 씬 내부의 오브젝트들의 Unity Event를 Invoke하는 클래스.
/// </summary>
public class UISelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public float InteractionTime = 2.0f;
	public UnityEvent[] UIEvents;

	private bool buttonPressed = false;
	private float pressTime = 0.0f;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!buttonPressed)
		{
			pressTime = Time.time;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        pressTime = 0.0f;
        buttonPressed = false;
    }

	private void Update()
	{
		if(!buttonPressed && pressTime > 0.0f)
		{
			if(Time.time - pressTime >= InteractionTime)
			{
				foreach(var UIEvent in UIEvents)
				{
					UIEvent?.Invoke();
				}
				buttonPressed = true;
			}
		}
	}
}
