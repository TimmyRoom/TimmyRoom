using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyTestUI : MonoBehaviour
{
    public TextMeshProUGUI time;

    [SerializeField] private LobbySceneManager lobbySceneManager;

    private void Update()
    {
        time.text = LobbySceneManager.GetCurrentTime();
    }

    public void EnterScene(int sceneIndex)
    {
        lobbySceneManager.MoveScene(sceneIndex);
    }
}
