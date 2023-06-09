using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자 데이터를 담당하는 클래스.
/// </summary>
public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
    int currentId = -1;
    public int CurrentProfile { get => currentId; set => currentId = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
        LoadData();
    }

    /// <summary>
    /// 모든 유저 정보를 불러온다.
    /// </summary>
    /// <returns>성공하면 true, 실패하면 false</returns>
    public bool LoadData()
    {
        // After Making Login Scene
        try
        {
            string dirPath = Application.persistentDataPath;
            string json = System.IO.File.ReadAllText(dirPath + "/userData.json");
            SerializableGameData gameData = JsonUtility.FromJson<SerializableGameData>(json);
            GameData.userDataList = gameData.userDataList;
            return true;
        }
        catch(System.Exception e)
        {
            System.IO.File.WriteAllText(Application.persistentDataPath + "/userData.json", "");
            Debug.LogWarning("LoadData : " + e.Message);
            return false;
        }
    }


    /// <summary>
    /// 지정된 프로필의 데이터 파일을 읽어 currentProfile을 갱신한다.
    /// </summary>
    /// <param name="targetProfile">프로필 이름.</param>
    public void ReadData(int targetProfile)
    {
        CurrentProfile = targetProfile;
        //TODO : 데이터를 읽어 적용한다.
        SceneRecorder.userId = CurrentProfile;
    }

    /// <summary>
    /// currenProfile에 해당하는 파일의 정보를 저장한다.
    /// </summary>
    public void SaveData()
    {
        SerializableGameData gameData = new SerializableGameData();
        gameData.userDataList = GameData.userDataList;

        string json = JsonUtility.ToJson(gameData);
        string dirPath = Application.persistentDataPath;
        // foreach(var user in GameData.userDataList)
        // {
        //     Debug.Log(user.id + " " + user.colorId + " " + user.patternId);
        // }
        System.IO.File.WriteAllText(dirPath + "/userData.json", json);
    }

    /// <summary>
    /// 새로운 프로필에 해당하는 폴더를 만든다.
    /// </summary>
    /// <param name="targetProfile">새로 생성할 프로필 이름.</param>
    /// <param name="jsonData">새로 생성할 프로필 정보.</param>
    public bool AddNewData(int colorId, int patterId)
    {
        int id = GameData.AddUser(colorId, patterId);
        if(id == -1)
        {
            Debug.LogWarning("AddNewData : AddUser Failed");
            return false;
        }
        CurrentProfile = id;
        return true;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application Quit");
        SaveData();
    }
}
