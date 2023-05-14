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


public static class GameData
{
    public static List<UserData> userDataList; 

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