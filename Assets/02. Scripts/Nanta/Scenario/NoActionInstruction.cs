using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoActionInstruction : MonoBehaviour, IScenario
{
    public Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>();
    }
}
