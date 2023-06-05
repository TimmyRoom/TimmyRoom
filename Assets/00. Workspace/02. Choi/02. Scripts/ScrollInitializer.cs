using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 스크롤 초기화
        ScrollRect scrollRect = GetComponent<ScrollRect>();

        scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, -150);
    }
}
