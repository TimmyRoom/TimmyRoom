using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject gazeObject;
    Image gaze; 
    Image image;

    Button button;
    bool isPointing = false;

    public float pointTimer = 0f, maxPointTime = 3f;

    [SerializeField]
    AnimationCurve curve;

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        gaze = gazeObject.GetComponent<Image>();
        image.color = new Color(0.75f, 0.75f, 0.75f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        isPointing = true;
        StartCoroutine(GoFront());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointing = false;
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(GoBack());
        gaze.fillAmount = 0f;
    }

    IEnumerator GoFront()
    {
        float c = 0.75f;
        float s = 1f;
        while(isPointing && c < 1f)
        {
            c = Mathf.Clamp( c + 0.025f, 0.75f, 1f);
            image.color = new Color(c, c, c, 1f);
            s = Mathf.Clamp( s + 0.02f, 1f, 1.2f);
            transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
        image.color = new Color(1f, 1f, 1f, 1f);
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    IEnumerator GoBack()
    {
        float c = 1f;
        float s = 1.2f;
        while(c > 0.75f)
        {
            c = Mathf.Clamp( c - 0.05f, 0.75f, 1f);
            image.color = new Color(c, c, c, 1f);
            s = Mathf.Clamp( s - 0.04f, 1f, 1.2f);
            transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
        image.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPointing)
        {
            pointTimer += Time.deltaTime;
            float t = pointTimer / maxPointTime;
            float fillAmount = curve.Evaluate(t);
            gaze.fillAmount = fillAmount;
            if(pointTimer >= maxPointTime)
            {
                button.onClick.Invoke();
                pointTimer = 0f;
            }
        }
        else
        {
            pointTimer = 0f;
        }
    }
}
