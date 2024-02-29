using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_OptionPopup : MonoBehaviour
{
    public void Open()
    {
        // 사운드 효과음이라던지
        // 초기화 함수
        gameObject.SetActive(true);
    }
    public void Close()
    {
        // 사운드 효과음이라던지
        // 여러가지
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnContinueButton()
    {
        Debug.Log("계속하기 버튼 클릭");
    }
    public void OnRestartButton()
    {
        Debug.Log("다시하기 버튼 클릭");

    }
    public void OnExitButton()
    {
        Debug.Log("게임종료 버튼 클릭");

    }
}
