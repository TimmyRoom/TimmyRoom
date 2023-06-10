using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : AbstractNantaInstrument
{
    /// <summary>
    /// 각 판정 영역에 대한 정보를 담고 있는 구조체.
    /// </summary>
    public SingleHitArea[] HitAreas;
    /// <summary>
    /// 등장 효과가 적용될 북의 모델링.
    /// </summary>
    public Transform[] DrumModels;
    /// <summary>
    /// 모델링의 기존 스케일.
    /// </summary>
    List<Vector3> originalScale = new List<Vector3>();
    public override void GetHitted(int type)
    {
        if(Judge.JudgeNote(type) == 1)
        {
            SoundManager.instance.PlaySound(InstrumentClips, InstrumentAudioSource, 0.42f);
        }
    }

    public override void Initialize()
    {
        foreach(var hitArea in HitAreas)
        {
            hitArea.action = () => { GetHitted(hitArea.InstrumentType); };
        }
        foreach(var drum in DrumModels)
        {
            originalScale.Add(drum.localScale);
        }
        Judge = FindAnyObjectByType<NantaJudgingLine>();
        StartCoroutine(DrumPop());
    }

    public override void OnDisappear()
    {
        StartCoroutine(DrumShrink());
    }

    IEnumerator DrumPop()
    {
        foreach(var drum in DrumModels)
        {
            drum.localScale = Vector3.zero;
        }
        for(float i = 0.1f; i <= 1f; i += 0.1f)
        {
            for(int j = 0; j < DrumModels.Length; j++)
            {
                DrumModels[j].localScale = originalScale[j] * i;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DrumShrink()
    {
        for(float i = 0.1f; i <= 1f; i += 0.1f)
        {
            for(int j = 0; j < DrumModels.Length; j++)
            {
                DrumModels[j].localScale = originalScale[j] * (1 - i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for(int j = 0; j < DrumModels.Length; j++)
        {
            DrumModels[j].localScale = originalScale[j];
        }
        gameObject.SetActive(false);
    }
}
