using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NantaJudgingLine : MonoBehaviour
{
    /// <summary>
    /// 노트가 생성된 후 판정면에 닿을 때까지의 시간.
    /// </summary>
    float judgingTime;
    public float JudgingTime { get => judgingTime; set => judgingTime = value; }
 
    /// <summary>
    /// 판정면. 노트가 해당 면에 Trigger된 동안은 성공 판정. 한번이라도 판정면에 Trigger된 후 Trigger에서 Exit될 경우 실패 판정.
    /// </summary>
    public BoxCollider JudjingLine;

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
        yield return new WaitForSeconds(Mathf.Clamp(time - judgingTime, 0, float.MaxValue));
        //노트를 NoteSpawnTransforms[type].position에 생성하여 NoteSpawnTransforms[type].rotation 방향으로 등속 운동시킨다.
    }

    /// <summary>
    /// 노트 타입에 연결되는 라인에서 노트 진행 방향으로 가장 멀리 이동한 노트를 참조해 판정을 한다.
    /// </summary>
    /// <param name="type">노트의 종류.</param>
    /// <returns>노트 판정 결과.</returns>
    public int JudgeNote(int type)
    {
        //노트 타입에 연결되는 라인에서 노트 진행 방향으로 가장 멀리 이동한 노트를 참조한다.
        //참조한 노트가 해당 면에 Trigger된 상태라면 1을 반환한다.
        //그 외엔 0을 반환한다.
        return 0;
    }
}
