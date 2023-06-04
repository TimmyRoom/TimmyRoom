using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceJudgingPoint : MonoBehaviour
{
    [SerializeField] float fallingTime = 2;
    public float FallingTime { get => fallingTime; set => fallingTime = value; }

    float noteVelocity = 0;
    public float NoteVelocity { get => noteVelocity; set => noteVelocity = value; }

    public Transform JudgePosition;

    public Transform NoteSpawnTransforms;

    public Rigidbody NotePrefab;

    private List<Rigidbody> notes;

    private List<IEnumerator> noteRoutines;

    private bool[] current;

    private void Start()
    {
        current = new bool[6];
    }

    public void SetVelocity()
    {
        NoteVelocity = (JudgePosition.position - NoteSpawnTransforms.position).magnitude / FallingTime;
    }

    public void SpawnNote(float time, int type)
    {
        IEnumerator noteRoutine = SpawnNoteRoutine(time, type);
        noteRoutines.Add(noteRoutine);
        StartCoroutine(noteRoutine);
    }

    IEnumerator SpawnNoteRoutine(float time, int type)
    {
        yield return new WaitForSeconds(Mathf.Clamp(time - FallingTime, 0, float.MaxValue));
        Rigidbody newNote = GetNote(type);
        newNote.velocity = NoteSpawnTransforms.forward * NoteVelocity;
        yield return new WaitForSeconds(1.1f * fallingTime);
        if (newNote.gameObject.activeInHierarchy) NantaScenarioManager.instance.JudgeNote(type, 0);
        Destroy(newNote?.gameObject);
        notes.Remove(newNote);
    }

    private void OnTriggerEnter(Collider other)
    {
        JudgeNote(other.gameObject.GetComponent<DanceNote>().type);
        notes.Remove(other.gameObject.GetComponent<Rigidbody>());
        other.gameObject.SetActive(false);
    }

    public void UsingTypeForScenario(int type)
    {
        if(DanceScenarioManager.instance.currentScenarioNum == 1)
        {
            DanceScenarioManager.instance.JudgeNote(type, 0);
        }
    }

    public int JudgeNote(int type)
    {
        int result = -1;

        Array.Copy(DanceScenarioManager.instance.danceAreaManager.area.isTriggered, current, 6);

        string leftValue = "-1";
        string rightValue = "-1";

        if (current[0]) leftValue = "1";
        else if (current[1]) leftValue = "2";
        else if (current[2]) leftValue = "3";

        if (current[3]) rightValue = "1";
        else if (current[4]) rightValue = "2";
        else if (current[5]) rightValue = "3";

        string curPose = leftValue + rightValue;

        if(curPose == type.ToString())
        {
            result = 1;
        }
        else
        {
            result = 0;
        }
        Debug.Log(curPose + ", result = " + result.ToString());
        DanceScenarioManager.instance.JudgeNote(type, result);
        //RaycastHit hit;
        //if (Physics.Raycast(RayPosition[type].position, RayPosition[type].forward, out hit))
        //{
        //    // 여기서 활성화된 Area를 바탕으로 해서, 판정을 내리도록 한다

        //    //if (hit.distance > 1.35f)
        //    //{
        //    //    result = 0;
        //    //}
        //    //else if (0f < hit.distance && hit.distance < 1.35f)
        //    //{
        //    //    result = 1;
        //    //    notes.Remove(hit.collider.gameObject.GetComponent<Rigidbody>());
        //    //    hit.collider.gameObject.SetActive(false);
        //    //    DanceScenarioManager.instance.JudgeNote(type, result);
        //    //}
        //    //else
        //    //{
        //    //    result = 0;
        //    //    notes.Remove(hit.collider.gameObject.GetComponent<Rigidbody>());
        //    //    hit.collider.gameObject.SetActive(false);
        //    //}
        //}
        //else
        //{
        //    DanceScenarioManager.instance.JudgeNote(type, result);
        //}
        //DanceScenarioManager.instance.JudgeNote(type, result);
        return result;
    }

    Rigidbody GetNote(int type)
    {
        Rigidbody newNote = Instantiate(NotePrefab, NoteSpawnTransforms.position, NoteSpawnTransforms.rotation);
        newNote.gameObject.GetComponent<DanceNote>().type = type;
        notes.Add(newNote);
        return newNote;
    }

    public void ResetAll()
    {
        foreach (IEnumerator routine in noteRoutines)
        {
            StopCoroutine(routine);
        }
        noteRoutines.Clear();
        foreach (Rigidbody note in notes)
        {
            Destroy(note.gameObject);
        }
        notes.Clear();
    }
}
