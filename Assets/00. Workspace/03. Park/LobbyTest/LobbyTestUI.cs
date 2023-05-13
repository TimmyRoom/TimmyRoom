using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyTestUI : MonoBehaviour
{
    public TextMeshProUGUI time;

    private void Update()
    {
        time.text = LobbyManager.ShowCurrentTime();
    }
}
