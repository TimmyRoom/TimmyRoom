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
    /// <summary>
    /// 악기와 관련된 효과음 목록.
    /// </summary>
    [SerializeField] public AudioClip InstrumentClip;
        
    /// <summary>
    /// 악기와 관련된 효과음이 나오는 곳.
    /// </summary>
    [SerializeField] public AudioSource InstrumentAudioSource;
    
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
        StartCoroutine(VibrateControl.instance.CustomVibrateLeft(0.2f, 0.2f));
        StartCoroutine(VibrateControl.instance.CustomVibrateRight(0.2f, 0.2f));
        SoundManager.instance.PlaySound(InstrumentClip, InstrumentAudioSource, 0.42f);
        NantaScenarioManager.instance.JudgeNote(1, 1);
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
        return new Dictionary<int, UnityAction>() { { 1, SetCount }};
    }
}
