using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 사용자가 컨트롤러 Ray를 통해 상호작용하여 캔버스 내부의 오브젝트들의 Unity Event를 Invoke하는 클래스.
/// </summary>
public class GazeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 현재 인터랙션까지 남은 시간을 시각적으로 알려주는 Gaze 이미지가 들어있는 오브젝트.
    /// </summary>
    [SerializeField] GameObject gazeObject;
    /// <summary>
    /// 현재 인터랙션까지 남은 시간을 시각적으로 알려주는 Gaze 이미지.
    /// </summary>
    Image gaze; 
    /// <summary>
    /// 버튼의 이미지.
    /// </summary>
    Image image;
    /// <summary>
    /// 포인터로 가리킬 시 확대되는 최대 크기.
    /// </summary>
    [Range(1f, 1.5f)]
    [SerializeField] float gazeScale = 1.2f;
    /// <summary>
    /// 포인터로 가리키지 않았을 때의 명도.
    /// </summary>
    [Range(0f, 1f)]
    [SerializeField] float ungazedColor = 0.75f;
    /// <summary>
    /// 버튼 컴포넌트.
    /// </summary>
    Button button;
    /// <summary>
    /// 사용자가 해당 오브젝트를 가리키고 있는지를 저장하는 bool 변수.
    /// </summary>
    bool isPointing = false;

    /// <summary>
    /// 사용자가 해당 오브젝트를 가리키고 있는 시간.
    /// </summary>
    float pointTimer = 0f;
    /// <summary>
    /// 사용자가 인터랙션을 위해 해당 오브젝트를 가리키고 있을 수 있는 최대 시간.
    /// </summary>
    public float maxPointTime = 3f;
    /// <summary>
    /// 인터랙션 시 Gaze가 변화하는 곡선.
    /// </summary>
    [SerializeField]
    AnimationCurve curve;
    /// <summary>
    /// 버튼의 원래 색상.
    /// </summary>
    Color originalColor;
    /// <summary>
    /// 프로그램 시작 시 image, button, gaze, originalColor를 초기화하고 버튼 색상 지정.
    /// </summary>
    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        gaze = gazeObject.GetComponent<Image>();
        originalColor = image.color;
        image.color = new Color(originalColor.r * 0.75f, originalColor.g * 0.75f, originalColor.b * 0.75f, 1f);
    }
    /// <summary>
    /// 포인터 Enter 시 isPointing을 true로 바꾸고 GoFront 코루틴을 실행.
    /// </summary>
    /// <param name="eventData">이벤트 데이터.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        isPointing = true;
        StartCoroutine(GoFront());
    }
    /// <summary>
    /// 포인터 Exit 시 isPointing을 false로 바꾸고 GoBack 코루틴을 실행, gaze 이미지의 fillAmount를 0으로 초기화.
    /// </summary>
    /// <param name="eventData">이벤트 데이터.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointing = false;
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(GoBack());
        gaze.fillAmount = 0f;
    }
    /// <summary>
    /// 포인터가 가리키고 있는 동안 Gaze 이미지와 버튼의 크기를 점점 키우는 코루틴.
    /// </summary>
    /// <returns></returns>
    IEnumerator GoFront()
    {
        float c = ungazedColor;
        float s = 1f;
        while(isPointing && c < 1f)
        {
            c = Mathf.Clamp( c + 0.025f, ungazedColor, 1f);
            image.color = new Color(originalColor.r * c, originalColor.g * c, originalColor.b * c, 1f);
            s = Mathf.Clamp( s + 0.02f, 1f, gazeScale);
            transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
        image.color = originalColor;
        transform.localScale = new Vector3(gazeScale, gazeScale, gazeScale);
    }
    /// <summary>
    /// 포인터가 가리키지 않는 동안 Gaze 이미지와 버튼의 크기를 점점 줄이는 코루틴.
    /// </summary>
    /// <returns></returns>
    IEnumerator GoBack()
    {
        float c = 1f;
        float s = gazeScale;
        while(c > ungazedColor)
        {
            c = Mathf.Clamp( c - 0.05f, ungazedColor, 1f);
            image.color = new Color(originalColor.r * c, originalColor.g * c, originalColor.b * c, 1f);
            s = Mathf.Clamp( s - 0.04f, 1f, gazeScale);
            transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
        image.color = new Color(originalColor.r * ungazedColor, originalColor.g * ungazedColor, originalColor.b * ungazedColor, 1f);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    /// <summary>
    /// 사용자가 버튼을 가리키고 있을 때, pointTimer를 증가시키고, pointTimer가 maxPointTime을 넘어가면 버튼의 onClick.Invoke()를 실행.
    /// </summary>
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
