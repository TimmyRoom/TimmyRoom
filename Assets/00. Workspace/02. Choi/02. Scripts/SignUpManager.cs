using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignUpManager : MonoBehaviour
{
    public static SignUpManager instance;
    SignUpUIManager signUpUIManager;

    public UserColor color;
    public UserPattern pattern;


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        signUpUIManager = GetComponent<SignUpUIManager>();
        Init();
    }
    
    void Init()
    {
        color = UserColor.None;
        pattern = UserPattern.None;

        signUpUIManager.Init();
    }

    public void SetColor(UserColor color)
    {
        this.color = color;
        signUpUIManager.SetColor(color);
    }

    public void ConfirmColor()
    {
        if(color == UserColor.None)
        {
            signUpUIManager.ShowNotice("색상을 선택해주세요.");
            return;
        }
        signUpUIManager.ConfirmColor();
    }

    public void SetPattern(UserPattern pattern)
    {
        this.pattern = pattern;
        signUpUIManager.SetPattern(pattern);
    }

    public void ConfirmPattern()
    {
        if(pattern == UserPattern.None)
        {
            signUpUIManager.ShowNotice("패턴을 선택해주세요.");
            return;
        } 
        signUpUIManager.ConfirmPattern();

        // 회원가입 완료
        bool isSignUpSucess = UserDataManager.instance.AddNewData((int)color, (int)pattern);
        UserDataManager.instance.SaveData();
        if(isSignUpSucess)
        {
            // 회원가입 성공
            Debug.Log("회원가입 성공");
            SceneMover.instance.MoveScene("Login");
        }
        else
        {
            // 회원가입 실패
            Init();
            signUpUIManager.ShowNotice("중복된 데이터가 있습니다.\n 다른 데이터를 선택해주세요.");
        }
    }
}
