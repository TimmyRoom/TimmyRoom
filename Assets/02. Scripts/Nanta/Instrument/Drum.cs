using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : AbstractNantaInstrument
{
    /// <summary>
    /// 각 판정 영역에 대한 정보를 담고 있는 구조체.
    /// </summary>
    public SingleHitArea[] HitAreas;
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
        Judge = FindAnyObjectByType<NantaJudgingLine>();
    }
}
