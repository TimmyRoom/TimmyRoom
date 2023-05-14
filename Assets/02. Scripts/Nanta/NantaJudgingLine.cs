using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NantaJudgingLine : MonoBehaviour
{
    [SerializeField] NantaScenarioManager manager;

    /// <summary>
    /// 노트가 생성된 후 판정면에 닿을 때까지의 시간.
    /// </summary>
    [SerializeField]float fallingTime;
    public float FallingTime { get => fallingTime; set => fallingTime = value; }

    /// <summary>
    /// 노트의 등속 운동 속도.
    /// </summary>
    [SerializeField]float noteVelocity;
    public float NoteVelocity { get => noteVelocity; set => noteVelocity = value; }

    List<float> JudgeDistance = new List<float>(){-10, 10};

    /// <summary>
    /// 판정을 위한 Raycast가 발생하는 Transform.
    /// </summary>
    public Transform[] RayPosition;

    /// <summary>
    /// 생성하는 노트의 프리팹.
    /// </summary>
    public Rigidbody NotePrefab;

    /// <summary>
    /// 노트가 생성되는 위치.
    /// </summary>
    public Transform[] NoteSpawnTransforms;

    /// <summary>
    /// SpawnNoteRoutine(time, type) 루틴 실행.
    /// </summary>
    /// <param name="time">루틴이 시작될 시간.</param>
    /// <param name="type">노트의 종류.</param>
    public void SpawnNote(float time, int type)
    {
        StartCoroutine(SpawnNoteRoutine(time, type));
    }

    /// <summary>
    /// 일정 시간동안 대기 후 노트를 생성하여 움직이게 한다.
    /// </summary>
    /// <param name="time">생성까지 대기하는 시간.</param>
    /// <param name="type">노트의 종류.</param>
    /// <returns></returns>
    IEnumerator SpawnNoteRoutine(float time, int type)
    {
        yield return new WaitForSeconds(Mathf.Clamp(time - fallingTime, 0, float.MaxValue));
        Rigidbody newNote = GetNote(type);
        newNote.velocity = NoteSpawnTransforms[type].forward * NoteVelocity;
    }

    /// <summary>
    /// 노트 타입에 연결되는 라인에서 노트 진행 방향으로 가장 멀리 이동한 노트를 참조해 판정을 한다.
    /// </summary>
    /// <param name="type">노트의 종류.</param>
    /// <returns>노트 판정 결과.</returns>
    public int JudgeNote(int type)
    {
        int result = 0;
        RaycastHit hit;
        if (Physics.Raycast(RayPosition[type].position, RayPosition[type].forward, out hit))
        {
            if(hit.distance > 3)
            {
                result = 0;
            }
            else if(-3 < hit.distance && hit.distance < 3)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            Destroy(hit.collider.gameObject);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1000f, Color.red);
        }
        return result;
    }

    Rigidbody GetNote(int type)
    {
        //TODO : 오브젝트 풀링 기법 구현
        Rigidbody newNote = Instantiate(NotePrefab, NoteSpawnTransforms[type].position, NoteSpawnTransforms[type].rotation);
        return newNote;
    }
}
