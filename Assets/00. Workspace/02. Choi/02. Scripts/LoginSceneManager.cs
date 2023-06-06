using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public static void MoveToSignUpScene()
    {
        SceneMover.instance.MoveScene("SignUp");
    }

    public static void MovetoLobbyScene()
    {
        SceneMover.instance.MoveScene("Lobby");
    }
}
