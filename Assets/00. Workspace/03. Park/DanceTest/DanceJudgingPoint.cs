using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceJudgingPoint : MonoBehaviour
{
    [SerializeField] float judgingTime = 2;
    public float JudgingTime { get => judgingTime; set => judgingTime = value; }

    float noteVelocity = 0;
    public float NoteVelocity { get => noteVelocity; set => noteVelocity = value; }

    public Transform[] RayPosition;

    public Transform[] JudgePosition;

    public Transform[] NoteSpawnTransforms;

    public Rigidbody NotePrefab;

    private List<Rigidbody> notes;

    private List<IEnumerator> noteRoutines;

    public void SetVelocity()
    {
        throw new System.NotImplementedException();
    }

    public void SpawnNote(float time, int[] types)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator SpawnNoteRoutine(float time, int[] types)
    {
        throw new System.NotImplementedException();
    }

    public int JudgeNote(int type)
    {
        throw new System.NotImplementedException();
    }

    public void ResetAll()
    {
        throw new System.NotImplementedException();
    }
}
