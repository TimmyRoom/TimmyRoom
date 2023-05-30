using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NantaJudgingLine : MonoBehaviour
{
    /// <summary>
    /// 노트가 생성된 후 판정면에 닿을 때까지의 시간.
    /// </summary>
    [SerializeField]float fallingTime = 2;
    public float FallingTime { get => fallingTime; set => fallingTime = value; }

    /// <summary>
    /// 노트의 등속 운동 속도.
    /// </summary>
    float noteVelocity = 0;
    public float NoteVelocity { get => noteVelocity; set => noteVelocity = value; }

    /// <summary>
    /// 판정을 위한 Raycast가 발생하는 Transform.
    /// </summary>
    public Transform[] RayPosition;

    /// <summary>
    /// 실제 사용자가 보는 판정선 Transform.
    /// </summary>
    public Transform[] JudgePosition;

    /// <summary>
    /// 생성하는 노트의 프리팹.
    /// </summary>
    public Rigidbody NotePrefab;

    /// <summary>
    /// 노트가 생성되는 위치.
    /// </summary>
    public Transform[] NoteSpawnTransforms;

    private List<Rigidbody> notes = new List<Rigidbody>();

    /// <summary>
    /// 실행되는 모든 노트 루틴을 저장하는 리스트.
    /// </summary>
    private List<IEnumerator> noteRoutines = new List<IEnumerator>();

    public void SetVelocity()
    {
        NoteVelocity = (JudgePosition[0].position - NoteSpawnTransforms[0].position).magnitude / FallingTime;
    }

    /// <summary>
    /// SpawnNoteRoutine(time, type) 루틴 실행.
    /// </summary>
    /// <param name="time">루틴이 시작될 시간.</param>
    /// <param name="type">노트의 종류.</param>
    public void SpawnNote(float time, int type)
    {
        IEnumerator noteRoutine = SpawnNoteRoutine(time, type);
        noteRoutines.Add(noteRoutine);
        StartCoroutine(noteRoutine);
    }

    /// <summary>
    /// 일정 시간동안 대기 후 노트를 생성하여 움직이게 한다.
    /// </summary>
    /// <param name="time">생성까지 대기하는 시간.</param>
    /// <param name="type">노트의 종류.</param>
    /// <returns></returns>
    IEnumerator SpawnNoteRoutine(float time, int type)
    {
        yield return new WaitForSeconds(Mathf.Clamp(time, 0, float.MaxValue));
        Rigidbody newNote = GetNote(type);
        newNote.velocity = NoteSpawnTransforms[type].forward * NoteVelocity;
        yield return new WaitForSeconds(1.1f * fallingTime);
        if(newNote.gameObject.activeInHierarchy) NantaScenarioManager.instance.JudgeNote(type, 0);
        Destroy(newNote?.gameObject);
        notes.Remove(newNote);
    }

    /// <summary>
    /// 노트 타입에 연결되는 라인에서 노트 진행 방향으로 가장 멀리 이동한 노트를 참조해 판정을 한다.
    /// </summary>
    /// <param name="type">노트의 종류.</param>
    /// <returns>노트 판정 결과.</returns>
    public int JudgeNote(int type)
    {
        int result = -1;
        RaycastHit hit;
        if (Physics.Raycast(RayPosition[type].position, RayPosition[type].forward, out hit))
        {
            if(hit.distance > 1.35f)
            {
                result = 0;
            }
            else if(0f < hit.distance && hit.distance < 1.35f)
            {
                result = 1;
                notes.Remove(hit.collider.gameObject.GetComponent<Rigidbody>());
                hit.collider.gameObject.SetActive(false);
                NantaScenarioManager.instance.JudgeNote(type, result);
            }
            else
            {
                result = 0;
                notes.Remove(hit.collider.gameObject.GetComponent<Rigidbody>());
                hit.collider.gameObject.SetActive(false);
            }
        }
        return result;
    }

    Rigidbody GetNote(int type)
    {
        Rigidbody newNote = Instantiate(NotePrefab, NoteSpawnTransforms[type].position, NoteSpawnTransforms[type].rotation);
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
