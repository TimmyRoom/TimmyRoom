using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneManager : AbstractSceneManager
{
    /// <summary>
    /// 현재 프로필과 매칭되는 이미지 스프라이트를 반환한다.
    /// </summary>
    public Sprite ShowProfile()
    {
        // ProfileManager 생성 시 함께 작업
        throw new NotImplementedException();
    }

    /// <summary>
    /// DataTime 라이브러리를 통해 현재 시각을 출력한다.
    /// 기준은 KST, 형식은 HHmmss.
    /// </summary>
    public static string GetCurrentTime()
    {
        DateTime currentDate = DateTime.Now;
        return currentDate.ToString("HH:mm:ss");
    }

    public override void MoveScene(string sceneName)
    {
        SceneMover.instance.MoveScene(sceneName);
    }

    public override void MoveScene(int sceneIndex)
    {
        SceneMover.instance.MoveScene(sceneIndex);
    }
}
