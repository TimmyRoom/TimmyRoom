using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
/// <summary>
/// 채보 데이터를 나타내는 클래스. JSON 파싱 후 해당 형태로 변환된다.
/// </summary>
public class GameChart
{
    /// <summary>
    /// 차트의 이름.
    /// </summary>
    public string ChartName;
    /// <summary>
    /// 음악의 이름.
    /// </summary>
    public string SongName;
    /// <summary>
    /// 아티스트의 이름.
    /// </summary>
    public string ArtistName;
    /// <summary>
    /// 음악의 길이.
    /// </summary>
    public float SongLength;
    /// <summary>
    /// 음악의 초기 Offset.
    /// </summary>
    public float Offset;
    /// <summary>
    /// 음악의 BPM.
    /// </summary>
    public float BPM;
    /// <summary>
    /// 음악에 등장하는 모든 노트들의 리스트.
    /// </summary>
    public List<Note> Notes;
}