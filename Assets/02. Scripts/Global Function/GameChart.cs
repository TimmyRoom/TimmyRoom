using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class GameChart
{
    public string ChartName;
    public string SongName;
    public string ArtistName;
    public string SongLength;
    public string Offset;
    public string BPM;
    public List<Note> Notes;
}