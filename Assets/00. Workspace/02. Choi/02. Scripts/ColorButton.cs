using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public UserColor color;

    private void Start() {
        this.transform.GetComponent<Image>().color = ColorManager.instance.GetColor((int)color);
    }

    public void SetColor()
    {
        SignUpManager.instance.SetColor(color);
    }
}
