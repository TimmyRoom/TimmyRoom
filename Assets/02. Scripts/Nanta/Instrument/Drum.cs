using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : AbstractNantaInstrument
{
    public SingleHitArea[] HitAreas;
    public override void GetHitted(int type)
    {
        Judge.JudgeNote(type);
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
