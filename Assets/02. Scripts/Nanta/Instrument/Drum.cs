using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : AbstractNantaInstrument
{
    /// <summary>
    /// 각 판정 영역에 대한 정보를 담고 있는 구조체.
    /// </summary>
    public SingleHitArea[] HitAreas;
    [Range(0, 1)]
    public float VibrateTime = 0.5f;
    [Range(0, 1)]
    public float VibrateAmplifier = 0.5f;
    public override void GetHitted(int type)
    {
        SoundManager.instance.PlaySound(InstrumentClips, InstrumentAudioSource, 0.4f);
        if(Judge.JudgeNote(type) == 0)
        {
            if(type == 0)
            {
                VibrateControl.instance.CustomVibrateLeft(VibrateAmplifier, VibrateTime);
            }
            else
            {
                VibrateControl.instance.CustomVibrateLeft(VibrateAmplifier, VibrateTime);
            }
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
