using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHitArea : AbstractNantaInstrument
{
    /// <summary>
    /// 악기의 타입 번호. 어떤 라인의 노트와 연결되는지 판단하는데 사용된다.
    /// </summary>
    public int InstrumentType;
    public override void Initialize()
    {
        return;
    }

    public override void GetHitted(int type)
    {
        int result = Judge.JudgeNote(type);
        if(result == 0)
        {
            //SoundManager.instance.SoundPlay(InstrumentClips[0], InstrumentAudioSource);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GetHitted(InstrumentType);
    }
}
