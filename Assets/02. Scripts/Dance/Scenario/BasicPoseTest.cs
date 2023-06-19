using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BasicPoseTest : MonoBehaviour, IScenario
{
    /// <summary>
    /// 초급 단계인지 확인하기 위한 변수.
    /// </summary>
    public int isBasic;

    /// <summary>
    /// 공지를 표시하는 텍스트.
    /// </summary>
    [SerializeField] protected TextMeshProUGUI notifyText;

    /// <summary>
    /// 해야할 포즈를 표시하는 이미지.
    /// </summary>
    [SerializeField] protected Image poseGuideImage;

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
    private (int, int) answer = (-1, -1);

    /// <summary>
    /// 시나리오를 초기화하는 함수.
    /// 초급 단계일 경우 판정 영역의 가이드를 활성화한다.
    /// </summary>
    public void Initialize()
    {
        isBasic = DanceScenarioManager.instance.currentScenarioNum;
        DanceScenarioManager.instance.danceJudgingPoint.JudgePointGuide.SetActive(false);
        if (isBasic == 1)
        {
            DanceScenarioManager.instance.danceAreaManager.area.EnableGuide();
            notifyText.text = "아래 동작을 따라해보세요!\n가이드에 손을 맞춰주세요.";
        }
        else
        {
            DanceScenarioManager.instance.danceAreaManager.area.DisableGuide();
            notifyText.text = "아래 동작을 따라해보세요!\n가이드 없이 해보도록 해요.";
        }
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
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[0];
                break;
            case 1:
                answer = (1, 2);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[1];
                break;
            case 2:
                answer = (1, 3);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[2];
                break;
            case 3:
                answer = (2, 1);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[3];
                break;
            case 4:
                answer = (2, 2);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[4];
                break;
            case 5:
                answer = (2, 3);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[5];
                break;
            case 6:
                answer = (3, 1);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[6];
                break;
            case 7:
                answer = (3, 2);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[7];
                break;
            case 8:
                answer = (3, 3);
                poseGuideImage.sprite = DanceScenarioManager.instance.danceJudgingPoint.sprites[8];
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
        while (answer == (-1, -1))
        {
            ; // Do Nothing in Loop
        }

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
                clear = 0;
                answer = (-1, -1);
                if (DanceScenarioManager.instance.currentScenarioNum == 1)
                {
                    DanceScenarioManager.instance.SetScenario(2);
                }
                else
                    DanceScenarioManager.instance.SetScenario(3);
            }
            else NewAnswer();
        }
    }

    public virtual Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 0, SetPoseText }, { 2, Initialize } };
    }
}
