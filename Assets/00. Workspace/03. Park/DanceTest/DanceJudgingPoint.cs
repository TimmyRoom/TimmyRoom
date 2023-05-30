using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceJudgingPoint : MonoBehaviour
{
    [SerializeField] float judgingTime = 2;
    public float JudgingTime { get => judgingTime; set => judgingTime = value; }

    public BoxCollider[] Triggers;

    public AudioClip[] InstrumentClips;

    public AudioSource InstrumentAudioSource;

    public Transform[] NoteSpawnTransforms;

    public void SpawnNote(float time, int[] types)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator SpawnNoteRoutine(float time, int[] types)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator PlayNoteRoutine()
    {
        throw new System.NotImplementedException();
    }

    public int JudgeNote(int type)
    {
        throw new System.NotImplementedException();
    }
}
