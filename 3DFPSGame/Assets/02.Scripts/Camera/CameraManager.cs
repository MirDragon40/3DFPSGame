using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum CameraMode
{
    FPS,
    TPS,
}
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set;}

    public FPSCamera fpsCamera;
    public TPSCamera tpsCamera;

    public CameraMode Mode = CameraMode.FPS;

    public float RotationSpeed = 200;  // 초당 200초까지 회전 가능한 속도
    // 누적할 x각도와 y 각도
    public float _mx = 0;
    public float _my = 0;

    void Start()
    {
        //fpsCamera = this.gameObject.GetComponent<FPSCamera>();
        //tpsCamera = this.gameObject.GetComponent<TPSCamera>();
        fpsCamera = GetComponent<FPSCamera>();
        tpsCamera = GetComponent<TPSCamera>();

        SetFPSCameraMode();
    }

    public void SetCameraMode(CameraMode mode)
    {
        //fpsCamera.enabled = (mode == CameraMode.FPS);
        //tpsCamera.enabled = (mode == CameraMode.TPS);
        Mode = mode;
    }


    private void Awake()
    {

        // 싱글톤 패턴 : 오직 한개의 클래스 인스턴스를 갖도록 보장
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);


    }

    public void SetFPSCameraMode()
    {
        fpsCamera.enabled = true;
        tpsCamera.enabled = false;
    }
    public void SetTPSCameramode()
    {
        fpsCamera.enabled = false;
        tpsCamera.enabled = true;

    }
}
