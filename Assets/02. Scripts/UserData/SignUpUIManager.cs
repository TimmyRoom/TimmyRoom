using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpUIManager : MonoBehaviour
{
    GameObject resultObject;
    Image colorImage, patternImage;

    GameObject confirmButton, confirmPatternObject;
    Image confirmColorImage;
    
    GameObject colorGrid, patternGrid;
   
   
    private void Start() {
        resultObject = this.transform.Find("Result").gameObject;
        colorImage = resultObject.transform.Find("Color").GetComponent<Image>();
        patternImage = resultObject.transform.Find("Pattern").GetComponent<Image>();
        
        confirmButton = this.transform.Find("Confirm").gameObject;
        GameObject result = confirmButton.transform.Find("Result").gameObject;
        confirmColorImage = result.transform.Find("Color").GetComponent<Image>();
        confirmPatternObject = result.transform.Find("PatternMask").Find("Pattern").gameObject;

        colorGrid = this.transform.Find("ColorGrid").gameObject;
        patternGrid = this.transform.Find("PatternGrid").gameObject;

        Init();
    }

    public void Init()
    {
        colorGrid.SetActive(true);
        patternGrid.SetActive(false);
        patternImage.gameObject.SetActive(false);
        patternImage.color = new Color(1, 1, 1, 0f);
        confirmPatternObject.SetActive(false);
        confirmButton.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmButton.GetComponent<Button>().onClick.AddListener(() => { SignUpManager.instance.ConfirmColor(); });
    }

    public void ShowNotice(string notice)
    {
        GameObject noticeObject = this.transform.Find("Notice").gameObject;
        noticeObject.SetActive(true);
        noticeObject.transform.Find("Text").GetComponent<Text>().text = notice;
    }

    public void SetColor(UserColor color)
    {
        colorImage.color = ColorManager.instance.GetColor((int)color);
        confirmColorImage.color = ColorManager.instance.GetColor((int)color);
    }

    public void ConfirmColor()
    {
        colorGrid.SetActive(false);
        patternGrid.SetActive(true);
        patternImage.gameObject.SetActive(true);
        confirmPatternObject.SetActive(true);


        confirmButton.transform.Find("Current State").Find("Text").GetComponent<Text>().text = "선택 \n 패턴";
        confirmButton.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmButton.GetComponent<Button>().onClick.AddListener(() => { SignUpManager.instance.ConfirmPattern(); });
    }

    public void SetPattern(UserPattern pattern)
    {
        patternImage.color = new Color(1, 1, 1, 1f);
        patternImage.sprite = PatternManager.instance.GetPattern((int)pattern);

        confirmPatternObject.GetComponent<UIPatternSetter>().SetPattern(pattern);
    }

    public void ConfirmPattern()
    {
        patternGrid.SetActive(false);
    }
}
