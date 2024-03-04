using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameManager.Instance.ContinueGame();

    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnContinueButton()
    {
        Debug.Log("계속하기 버튼 클릭");

        Close();
    }
    public void OnResumeButton()
    {
        // 씬매니저야. (현재 열려 있는 씬)번 씬을 로드해라
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
        Debug.Log("다시하기 버튼 클릭");


    }
    public void OnExitButton()
    {
        Debug.Log("게임종료 버튼 클릭");

        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터 에서 실행했을 경우 종료하는 방법
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
