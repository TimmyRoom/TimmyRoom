using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject gazeObject;
    Image gaze; 

    Button button;
    bool isPointing = false;

    public float pointTimer = 0f, maxPointTime = 3f;

    [SerializeField]
    AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        button = this.transform.GetComponent<Button>();
        gazeObject = this.transform.Find("Gaze").gameObject;
        gaze = gazeObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        isPointing = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointing = false;
        EventSystem.current.SetSelectedGameObject(null);
        gaze.fillAmount = 0f;
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
