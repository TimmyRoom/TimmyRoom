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

    public List<Sprite> sprites = new List<Sprite>(); 

    private List<Rigidbody> notes = new List<Rigidbody>();

    private List<IEnumerator> noteRoutines = new List<IEnumerator>();

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
        if (newNote.gameObject.activeInHierarchy) DanceScenarioManager.instance.JudgeNote(type, 0);
        Destroy(newNote?.gameObject);
        notes.Remove(newNote);
    }

    private void OnTriggerEnter(Collider other)
    {
        JudgeNote(other.gameObject.GetComponent<DanceNote>().type);
        notes.Remove(other.gameObject.GetComponent<Rigidbody>());
        other.gameObject.SetActive(false);
    }

    public void UsingTypeForScenario()
    {
        switch (DanceScenarioManager.instance.currentScenarioNum)
        {
            case 1:
                DanceScenarioManager.instance.JudgeNote(11, 1);
                break;
            case 2:
                DanceScenarioManager.instance.JudgeNote(11, 1);
                break;
            default:
                break;
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

        //if (DanceScenarioManager.instance.danceAreaManager.area.isTriggered[0]) leftValue = "1";
        //else if (DanceScenarioManager.instance.danceAreaManager.area.isTriggered[1]) leftValue = "2";
        //else if (DanceScenarioManager.instance.danceAreaManager.area.isTriggered[2]) leftValue = "3";

        //if (DanceScenarioManager.instance.danceAreaManager.area.isTriggered[3]) rightValue = "1";
        //else if (DanceScenarioManager.instance.danceAreaManager.area.isTriggered[4]) rightValue = "2";
        //else if (DanceScenarioManager.instance.danceAreaManager.area.isTriggered[5]) rightValue = "3";

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
        
        return result;
    }

    Rigidbody GetNote(int type)
    {
        Rigidbody newNote = Instantiate(NotePrefab, NoteSpawnTransforms.position, NoteSpawnTransforms.rotation);
        newNote.gameObject.GetComponent<DanceNote>().type = type;
        switch (type)
        {
            case 11:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[0];
                break;
            case 12:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[1];
                break;
            case 13:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[2];
                break;
            case 21:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[3];
                break;
            case 22:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[4];
                break;
            case 23:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[5];
                break;
            case 31:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[6];
                break;
            case 32:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[7];
                break;
            case 33:
                newNote.gameObject.GetComponent<DanceNote>().image.sprite = sprites[8];
                break;
            default:
                break;
        }
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
