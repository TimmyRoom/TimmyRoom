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
    bool LoadData()
    {
        try
        {
            string dirPath = Application.persistentDataPath;
            string json = System.IO.File.ReadAllText(dirPath + "/userData.json");
            GameData.userDataList = JsonUtility.FromJson<List<UserData>>(json);
            return true;
        }
        catch(UnityEngine.UnityException e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }


    /// <summary>
    /// 지정된 프로필의 데이터 파일을 읽어 currentProfile을 갱신한다.
    /// </summary>
    /// <param name="targetProfile">프로필 이름.</param>
    void ReadData(int targetProfile)
    {
        CurrentProfile = targetProfile;
        //TODO : 데이터를 읽어 적용한다.
        SceneRecorder.userId = CurrentProfile;
    }

    /// <summary>
    /// currenProfile에 해당하는 파일의 정보를 저장한다.
    /// </summary>
    void SaveData()
    {
        string json = JsonUtility.ToJson(GameData.userDataList);
        string dirPath = Application.persistentDataPath;
        System.IO.File.WriteAllText(dirPath + "/userData.json", json);
    }

    /// <summary>
    /// 새로운 프로필에 해당하는 폴더를 만든다.
    /// </summary>
    /// <param name="targetProfile">새로 생성할 프로필 이름.</param>
    /// <param name="jsonData">새로 생성할 프로필 정보.</param>
    void AddNewData(int colorId, int patterId)
    {
        int id = GameData.AddUser(colorId, patterId);
        CurrentProfile = id;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
