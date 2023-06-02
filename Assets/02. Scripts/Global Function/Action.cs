using System;

[System.Serializable]
/// <summary>
/// Note에서 다루는 각 노트의 액션.
/// </summary>
public class Action
{
    /// <summary>
    /// 액션의 이름.
    /// </summary>
    public string Name;
    /// <summary>
    /// 액션의 타입.
    /// </summary>
    public int Type;
}