using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPatternSetter : MonoBehaviour
{
    public void SetPattern(UserPattern pattern)
    {
        if(gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            transform.GetChild(i).GetComponent<Image>().sprite = PatternManager.instance.GetPattern((int)pattern);
        }
    }
}
