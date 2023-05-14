using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class RecordInfo
{
    public RecordType type;
    public RecordSceneType sceneType;
}

[System.Serializable]
public enum RecordType
{
    Image,
    Video
}

[System.Serializable]
public enum RecordSceneType
{
    Nanta,
    Dance
}