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

    public float RotationSpeed = 200;  // �ʴ� 200�ʱ��� ȸ�� ������ �ӵ�
    // ������ x������ y ����
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

        // �̱��� ���� : ���� �Ѱ��� Ŭ���� �ν��Ͻ��� ������ ����
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
