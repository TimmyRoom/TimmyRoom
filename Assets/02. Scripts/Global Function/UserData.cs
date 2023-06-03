using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UserData
{
    public int id;
    public int colorId;
    public int patternId;
}

[System.Serializable]
public enum UserColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple,
    Orange,
    Black,
    SkyBlue,
    Brown,
    Pink,
    Lime,
    Olive,
    Navy,
    None
}

[System.Serializable]
public enum UserPattern
{
    Star,
    Heart,
    Circle,
    Square,
    Music,
    Triangle,
    Car,
    Rocket,
    None,
}

[System.Serializable]
public class SerializableGameData
{
    public List<UserData> userDataList = new List<UserData>();
}

public static class GameData
{
    public static List<UserData> userDataList = new List<UserData>(); 

    public static int AddUser(int colorId, int patternId)
    {
        int id = -1;
        if(CheckPurity(colorId, patternId))
        {
            UserData userData = new UserData();
            userData.id = userDataList.Count;
            userData.colorId = colorId;
            userData.patternId = patternId;
            userDataList.Add(userData);
            id = userData.id;
        }
        return id;
    }

    /// <summary>
    /// 동일한 컬러랑 패턴 아이디를 고른 아이디가 존재하는지 확인한다.
    /// </summary>
    /// <returns>존재하면 true, 아니면 false.</returns>
    public static bool CheckPurity(int colorId, int patternId)
    {
        bool result = true;
        foreach(UserData userData in userDataList)
        {
            if(userData.colorId == colorId && userData.patternId == patternId)
            {
                result = false;
                break;
            }
        }
        return result;
    }    
}