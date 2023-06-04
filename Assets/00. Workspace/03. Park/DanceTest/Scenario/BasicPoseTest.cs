using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BasicPoseTest : MonoBehaviour, IScenario
{
    [SerializeField] protected TextMeshProUGUI notifyText;
    [SerializeField] protected TextMeshProUGUI poseText;
    private bool[] current;
    private int clear = 0;
    private (int, int) answer;

    public virtual void Start()
    {
        notifyText.text = "Follow the Guide!";
        current = new bool[6];
        NewAnswer();
    }

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
                //DanceScenarioManager.instance.SetScenario(2);
            }
            else NewAnswer();
        }
    }

    public virtual Dictionary<int, UnityAction> GetActions()
    {
        return new Dictionary<int, UnityAction>() { { 1, SetPoseText } };
    }
}
