using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoActionInstruction : MonoBehaviour, IInstruction
{
    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>();
    }
}
