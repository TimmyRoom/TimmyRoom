using System;
using System.Collections.Generic;

[System.Serializable]
/// <summary>
/// GameChart에서 다루는 각 노트의 클레스.
/// </summary>
public class Note
{
    /// <summary>
    /// 노트가 등장하는 시간.
    /// </summary>
    public float Time;
    /// <summary>
    /// 노트에 포함된 액션 목록.
    /// </summary>
    public List<Action> Actions;
}