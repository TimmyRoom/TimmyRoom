using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class GameChart
{
    public string ChartName;
    public string SongName;
    public string ArtistName;
    public float SongLength;
    public float Offset;
    public float BPM;
    public List<Note> Notes;
}