using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginCanvasManager : MonoBehaviour
{
    public GameObject profilePrefab;
    public GameObject profileGrid;

    UserDataManager userDataManager;

    private void start()
    {
        userDataManager = UserDataManager.instance;
    }

    private void Start() {
        foreach(var user in GameData.userDataList)
        {
            GameObject profile = Instantiate(profilePrefab, profileGrid.transform, false);
            profile.GetComponent<Profile>().SetProfile(user.id, user.colorId, user.patternId);
        }
    }

    public void NewStart()
    {
        foreach(var user in GameData.userDataList)
        {
            GameObject profile = Instantiate(profilePrefab, profileGrid.transform, false);
            profile.GetComponent<Profile>().SetProfile(user.id, user.colorId, user.patternId);
        }
    }

}
