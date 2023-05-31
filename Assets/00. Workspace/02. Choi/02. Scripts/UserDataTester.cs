using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataTester : MonoBehaviour
{
    public void AddUser()
    {
        int colorId = Random.Range(0, ColorManager.instance.colors.Length);
        int patternId = Random.Range(0, PatternManager.instance.patterns.Length);
        UserDataManager.instance.AddNewData(colorId, patternId);

        Debug.Log(GameData.userDataList.Count);
    }
}
