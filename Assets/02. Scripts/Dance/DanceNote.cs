using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// 댄스 씬에서 판정과 노트 스프라이트 적용을 위해 타입과 스프라이트를 관리하는 자체 클래스.
/// </summary>
public class DanceNote : MonoBehaviour
{
    public int type;
    public int Type { get => type; set => type = value; }

    public Image image;

    private void Awake()
    {
        image = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
    }
}
