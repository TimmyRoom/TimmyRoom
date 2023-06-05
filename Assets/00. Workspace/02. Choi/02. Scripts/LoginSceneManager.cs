using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public void MoveToSignUpScene()
    {
        SceneMover.instance.MoveScene("SignUp");
    }
}
