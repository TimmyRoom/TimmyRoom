using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HitTest : MonoBehaviour, IScenario
{
    /// <summary>
    /// 카운트를 표시하는 텍스트.
    /// </summary>
    [SerializeField] protected TextMeshProUGUI text;
    /// <summary>
    /// 카운트를 저장하는 변수.
    /// </summary>
    [SerializeField] protected int count = 7;
    public virtual void Start()
    {
        text.text = count.ToString();
    }
    /// <summary>
    /// 노트가 적중되었을 때 호출되는 함수. 카운트를 증가시키고 인터페이스를 갱신한다.
    /// </summary>
    public virtual void SetCount()
    {
        text.text = (--count).ToString();
        if(count == 0)
        {
            NantaScenarioManager.instance.SetScenario(2);
        }
    }
    /// <summary>
    /// 해당 인스트럭션에서 발생하는 액션들의 리스트를 반환한다.
    /// </summary>
    /// <returns>인스트럭션에서 발생하는 액션들의 리스트</returns>
    public virtual Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 1, SetCount }, { 3, SetCount } };
    }
}
