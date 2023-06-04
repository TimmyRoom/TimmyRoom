using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float solidTime = 3f, fadeTime = 1f;
    Coroutine coroutine;
    CanvasGroup canvasGroup;

    private void OnEnable() {
        canvasGroup = GetComponent<CanvasGroup>();
        
        if(coroutine != null)
            StopCoroutine(coroutine);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut() {
        yield return new WaitForSeconds(solidTime);
        canvasGroup.alpha = 1f;
        while (canvasGroup.alpha > 0f) {
            canvasGroup.alpha -= Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}
