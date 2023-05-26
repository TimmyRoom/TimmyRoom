using System.Collections.Generic;
using UnityEngine.Events;

public interface IScenario
{
    /// <summary>
    /// 해당 인스트럭션에서 발생하는 액션들의 리스트를 반환한다.
    /// </summary>
    /// <returns>인스트럭션에서 발생하는 액션들의 리스트</returns>
    public Dictionary<int, UnityAction> GetActions();
}
