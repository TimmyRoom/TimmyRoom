using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternButton : MonoBehaviour
{
    public UserPattern pattern;

    private void Start()
    {
        this.transform.GetChild(0).GetComponent<Image>().sprite = PatternManager.instance.GetPattern((int)pattern);
        this.GetComponent<Button>().onClick.AddListener(() => { SetPattern(); });
    }


    public void SetPattern()
    {
        SignUpManager.instance.SetPattern(pattern);
    }
}
