using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public int id;
    public int colorId;
    public int patternId;

    public void SetProfile(int id, int colorId, int patternId)
    {
        this.id = id;
        this.colorId = colorId;
        this.patternId = patternId;

        this.GetComponent<Image>().color = ColorManager.instance.GetColor(colorId);
        this.transform.GetChild(0).GetComponent<Image>().sprite = PatternManager.instance.GetPattern(patternId);
        this.transform.Find("Text").GetComponent<Text>().text = "플레이어 " + (id + 1);
    }

    public void ClickProfile()
    {
        UserDataManager.instance.CurrentProfile = id;
        ModelManager.instance.SetProfile(colorId, patternId);
        LoginSceneManager.MovetoLobbyScene();
    }
}
