using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시간에 따라 노트를 생성하고 이를 통해 사용자에게 타이밍을 인지시키는 클래스.
/// </summary>
public class DanceJudgingPoint : MonoBehaviour
{
    [SerializeField] float fallingTime = 2;
    /// <summary>
    /// 노트가 생성된 후 판정면에 닿을 때까지의 시간.
    /// </summary>
    public float FallingTime { get => fallingTime; set => fallingTime = value; }

    float noteVelocity = 0;
    /// <summary>
    /// 노트의 등속 운동 속도.
    /// </summary>
    public float NoteVelocity { get => noteVelocity; set => noteVelocity = value; }

    /// <summary>
    /// 실제 사용자가 보는 판정선 Transform.
    /// </summary>
    public Transform JudgePosition;

    /// <summary>
    /// 사용자들이 눈으로 확인 가능한 판정 범위 오브젝트.
    /// </summary>
    public GameObject JudgePointGuide;

    /// <summary>
    /// 노트가 생성되는 위치.
    /// </summary>
    public Transform NoteSpawnTransforms;

    /// <summary>
    /// 생성하는 노트의 프리팹.
    /// </summary>
    public Rigidbody NotePrefab;

    /// <summary>
    /// 노트의 포즈 스프라이트들을 저장하는 리스트.
    /// </summary>
    public List<Sprite> sprites = new List<Sprite>();

    /// <summary>
    /// 생성된 모든 노트들을 저장하는 리스트.
    /// </summary>
    private List<Rigidbody> notes = new List<Rigidbody>();

    /// <summary>
    /// 실행되는 모든 노트 루틴을 저장하는 리스트.
    /// </summary>
    private List<IEnumerator> noteRoutines = new List<IEnumerator>();

    /// <summary>
    /// 판정을 위해 현재 발동된 트리거 정보를 자체적으로 저장하는 bool 배열.
    /// </summary>
    private bool[] current = new bool[6];

    /// <summary>
    /// 노트의 속력을 설정한다.
    /// </summary>
    public void SetVelocity()
    {
        NoteVelocity = (JudgePosition.position - NoteSpawnTransforms.position).magnitude / FallingTime;
    }

    /// <summary>
    /// SpawnNoteRoutine(time, type) 루틴 실행.
    /// </summary>
    /// <param name="time">루틴이 시작될 시간.</param>
    /// <param name="type">노트가 생성되는 라인</param>
    public void SpawnNote(float time, int type)
    {
        IEnumerator noteRoutine = SpawnNoteRoutine(time, type);
        noteRoutines.Add(noteRoutine);
        StartCoroutine(noteRoutine);
    }

    /// <summary>
    /// SpawnNoteRoutineWithChange(time, type) 루틴 실행.
    /// </summary>
    /// <param name="time">루틴이 시작될 시간.</param>
    /// <param name="type">노트가 생성되는 라인</param>
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

    /// <summary>
    /// 판정 콜라이더에 노트가 들어오면 해당 노트의 판정과 삭제를 수행함.
    /// </summary>
    /// <param name="other">콜라이더와 접촉한 노트 오브젝트</param>
    private void OnTriggerEnter(Collider other)
    {
        JudgeNote(other.gameObject.GetComponent<DanceNote>().type);
        notes.Remove(other.gameObject.GetComponent<Rigidbody>());
        other.gameObject.SetActive(false);
    }

    /// <summary>
    /// 노트 판정이 필요없는 시나리오를 위한 자체 판정 요청을 수행함.
    /// 포즈를 따라하기만 하는 시나리오 1, 2에서 사용됨.
    /// </summary>
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

    /// <summary>
    /// 판정면에 도달한 노트 오브젝트를 참조해 판정을 한다.
    /// 트리거 영역을 다루는 Manager에서 활성화 여부 배열을 받아와 판정에 이용한다.
    /// </summary>
    /// <param name="type">노트의 종류.</param>
    /// <returns>노트 판정 결과.</returns>
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
        
        return result;
    }

    /// <summary> 
    /// 노트를 생성하고 해당 오브젝트의 RigidBody 컴포넌트를 반환한다.
    /// 이 때, 노트의 타입에 맞는 스프라이트를 노트에 적용한다.
    /// <returns>생성된 노트의 RigidBody.</returns>
    /// </summary>
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

    /// <summary>
    /// 씬 시작 상태로 되돌리는 함수.
    /// </summary>
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
