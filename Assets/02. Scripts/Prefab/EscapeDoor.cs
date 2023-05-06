using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 비상탈출구 오브젝트 클래스.
/// </summary>
public class EscapeDoor : MonoBehaviour
{
    public UnityEvent[] Events;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            foreach(var doorEvent in Events)
            {
                doorEvent?.Invoke();
            }
        }
    }
}
