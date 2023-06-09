using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BasicPoseTest : MonoBehaviour, IScenario
{
    /// <summary>
    /// 초급 단계인지 확인하기 위한 변수.
    /// </summary>
    public bool isBasic = true;

    /// <summary>
    /// 공지를 표시하는 텍스트.
    /// </summary>
    [SerializeField] protected TextMeshProUGUI notifyText;
    /// <summary>
    /// 해야할 포즈를 표시하는 텍스트.
    /// </summary>
    [SerializeField] protected TextMeshProUGUI poseText;

    /// <summary>
    /// 판정을 위해 현재 발동된 트리거 정보를 자체적으로 저장하는 bool 배열.
    /// </summary>
    private bool[] current;
    /// <summary>
    /// 정답 횟수를 카운트하는 변수.
    /// </summary>
    private int clear = 0;
    /// <summary>
    /// 정답 정보를 저장하는 튜플.
    /// </summary>
    private (int, int) answer;

    public virtual void Start()
    {
        DanceScenarioManager.instance.danceJudgingPoint.JudgePointGuide.SetActive(false);
        if (isBasic)
            DanceScenarioManager.instance.danceAreaManager.area.EnableGuide();
        else
            DanceScenarioManager.instance.danceAreaManager.area.DisableGuide();
        notifyText.text = "Follow the Guide!";
        current = new bool[6];
        NewAnswer();
    }

    /// <summary>
    /// 정답 정보를 갱신하고 텍스트를 업데이트하는 함수.
    /// </summary>
    public void NewAnswer()
    {
        switch (clear)
        {
            case 0:
                answer = (1, 1);
                poseText.text = "L Down, R Down";
                break;
            case 1:
                answer = (1, 2);
                poseText.text = "L Down, R Mid";
                break;
            case 2:
                answer = (1, 3);
                poseText.text = "L Down, R Up";
                break;
            case 3:
                answer = (2, 1);
                poseText.text = "L Mid, R Down";
                break;
            case 4:
                answer = (2, 2);
                poseText.text = "L Mid, R Mid";
                break;
            case 5:
                answer = (2, 3);
                poseText.text = "L Mid, R Up";
                break;
            case 6:
                answer = (3, 1);
                poseText.text = "L Up, R Down";
                break;
            case 7:
                answer = (3, 2);
                poseText.text = "L Up, R Mid";
                break;
            case 8:
                answer = (3, 3);
                poseText.text = "L Up, R Up";
                break;
            case 9:
                break;
        }
    }

    /// <summary>
    /// 판정을 통해 정답 여부를 확인하고, 정답일 경우를 처리하는 함수.
    /// </summary>
    public virtual void SetPoseText()
    {
        Array.Copy(DanceScenarioManager.instance.danceAreaManager.area.isTriggered, current, 6);

        int leftValue = -1;
        int rightValue = -1;

        if (current[0]) leftValue = 1;
        else if (current[1]) leftValue = 2;
        else if (current[2]) leftValue = 3;

        if (current[3]) rightValue = 1;
        else if (current[4]) rightValue = 2;
        else if (current[5]) rightValue = 3;

        if ((leftValue, rightValue) == answer)
        {
            clear++;
            if(clear == 9)
            {
                poseText.text = "Great!";
                if(DanceScenarioManager.instance.currentScenarioNum == 1)
                    DanceScenarioManager.instance.SetScenario(2);
                else
                    DanceScenarioManager.instance.SetScenario(3);
            }
            else NewAnswer();
        }
    }

    public virtual Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetPoseText } };
    }
}
