using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자 데이터를 담당하는 클래스.
/// </summary>
public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
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
    }

    string currentProfile;
    public string CurrentProfile { get => currentProfile; set => currentProfile = value; }

    /// <summary>
    /// 모든 프로필에 대해 데이터 저장 위치를 읽는다.
    /// </summary>
    /// <returns>모든 프로필의 저장 위치.</returns>
    string[] GetAllDataLocations()
    {
        return null;
    }

    /// <summary>
    /// 지정된 프로필의 데이터 파일을 읽어 currentProfile을 갱신한다.
    /// </summary>
    /// <param name="targetProfile">프로필 이름.</param>
    void ReadData(string targetProfile)
    {
        CurrentProfile = targetProfile;
        SceneRecorder.instance.SetPath(CurrentProfile);
    }

    /// <summary>
    /// currenProfile에 해당하는 파일의 정보를 저장한다.
    /// </summary>
    void SaveData()
    {

    }

    /// <summary>
    /// 새로운 프로필에 해당하는 폴더를 만든다.
    /// </summary>
    /// <param name="targetProfile">새로 생성할 프로필 이름.</param>
    /// <param name="jsonData">새로 생성할 프로필 정보.</param>
    void AddNewData(string targetProfile, string jsonData)
    {
        CurrentProfile = targetProfile;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
